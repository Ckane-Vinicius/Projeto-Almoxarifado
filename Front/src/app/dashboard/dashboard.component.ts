
// -------------------------------
// file: ddl-cadastro.component.ts
// -------------------------------
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { DdlCadastroService, CnpjRecord, ApiRecord, Parcel } from './dashboard.service';
import { catchError, debounceTime, distinctUntilChanged, finalize, switchMap } from 'rxjs/operators';
import { of, Subject, Subscription } from 'rxjs';

interface EmpresaItem {
  key: string;
  empresa: string;
  cnpjEmpresa: string; // CNPJ da empresa (o campo que deve ser enviado como cnpjEmpresa)
}

@Component({
  selector: 'app-ddl-cadastro',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})

export class DashboardComponent implements OnInit, OnDestroy {
  recordsMap: { [k: string]: CnpjRecord } = {};
  recordsList: CnpjRecord[] = [];
  recordsListFull: any[] = [];
  searchCnpj = '';
  expanded: { [cnpj: string]: boolean } = {};


  searchControl = new FormControl('');
  loading = false;
  private searchSub!: Subscription;
  private refresh$ = new Subject<void>();
  quantidade: number = 0;

  form!: FormGroup;

  // lista fixa de 4 empresas (exemplo)
  empresas: EmpresaItem[] = [
    { key: 'emp1', empresa: 'Arrisca Comunicação Visual', cnpjEmpresa: '17.854.153/0001-07' },
    { key: 'emp2', empresa: 'Arrisca Encadernações', cnpjEmpresa: '03.141.927/0001-30' },
    { key: 'emp3', empresa: 'Arrisca Placas',  cnpjEmpresa: '46.459.612/0001-48' },
    { key: 'emp4', empresa: 'Arrisca Decora',  cnpjEmpresa: '52.735.390/0001-41' }
  ];

  constructor(private service: DdlCadastroService, private fb: FormBuilder) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      empresaSelect: ['', Validators.required], // chave da empresa selecionada
      empresa: [''],         // será preenchido automaticamente (nome)
      cnpjEmpresa: [''],     // preenchido automaticamente (CNPJ da empresa)
      cnpj: ['', Validators.required], // cnpj do destinatário (preenchido manualmente)
      razaosocial: [''],
      valor: ['', Validators.required],

      quantidadeParcelas: [1, [Validators.required, Validators.min(1)]],
      ddls: this.fb.array([], Validators.required)     
    });

    this.form.get('empresaSelect')!.valueChanges.subscribe((key) => {
      this.onEmpresaSelect(key);
    });

    this.setQuantidadeParcelas(1);

    // inicial load
    this.loadAll();

    // subscribe ao campo de busca com debounce
    this.searchSub = this.searchControl.valueChanges.pipe(
      debounceTime(400),
      distinctUntilChanged(),
      switchMap(q => {
        this.loading = true;
        const query = (q || '').trim();
        if (!query) {
          return this.service.getAll().pipe(catchError(()=> of([])));
        }
        return this.service.searchByCnpj(query).pipe(catchError(()=> of([])));
      })
    ).subscribe((list: ApiRecord[]) => {
      this.loading = false;
      // caso backend não filtre, o cliente pode filtrar manualmente:
      const q = (this.searchControl.value || '').replace(/\D/g,'');
      const filtered = q ? list.filter(x => (x.cnpj || '').replace(/\D/g,'').includes(q)) : list;
      this.buildMap(filtered);
    });
  }

  ngOnDestroy(): void {
    if (this.searchSub) this.searchSub.unsubscribe();
  }  

  get ddls(): FormArray {
    return this.form.get('ddls') as FormArray;
  }

  private createDdlGroup() {
    return this.fb.group({
      ddl: ['', [Validators.required, Validators.min(0)]]
    });
  }


  setQuantidadeParcelas(qtd: number) {
    // sempre trabalhar com número inteiro >=0
    const n = Math.max(0, Math.floor(Number(qtd) || 0));
    // limpar primeiro (mantém referência do FormArray)
    while (this.ddls.length !== 0) {
      this.ddls.removeAt(0);
    }
    // adicionar n grupos
    for (let i = 0; i < n; i++) {
      this.ddls.push(this.createDdlGroup());
    }
  }

  onQuantidadeChange(): void {
    const qtd = this.form.get('quantidadeParcelas')?.value || 0;

    // limpa o array primeiro
    this.ddls.clear();

    // adiciona inputs conforme a quantidade digitada
    for (let i = 0; i < qtd; i++) {
      this.ddls.push(this.createDdlGroup());
    }
  }

  onEmpresaSelect(key: string | null) {
    if (!key) {
      this.form.patchValue({ empresa: '', cnpjEmpresa: '' });
      return;
    }
    const selected = this.empresas.find(e => e.key === key);
    if (selected) {
      this.form.patchValue({
        empresa: selected.empresa,
        cnpjEmpresa: selected.cnpjEmpresa
      });
    } else {
      this.form.patchValue({ empresa: '', cnpjEmpresa: '' });
    }
  }

  onSubmit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
        const payload = {
          empresa: this.form.value.empresa,
          cnpjempresa: this.form.value.cnpjEmpresa,
          cnpj: this.form.value.cnpj,
          razaosocial: this.form.value.razaosocial,
          valor: String(this.form.value.valor),
          ddl: this.ddls.controls.map(g => Number(g.get('ddl')?.value || 0))
        };

        this.loading = true;
        this.service.addParcel(payload).subscribe({
          next: (res: any) => {
            this.loading = false;
            alert(typeof res === 'string' ? res : JSON.stringify(res));
            // limpa ddls mantendo quantidade inicial
            const qty = Number(this.form.get('quantidadeParcelas')?.value || 1);
            this.setQuantidadeParcelas(qty);
            this.form.patchValue({ cnpj: '', razaosocial: '', valor: '' });
          },
          error: (err) => {
            this.loading = false;
            console.error(err);
            alert('Erro ao enviar');
          }
    });
  }
    
  

  loadAll() {
    this.loading = true;
    this.service.getAll()      
       .pipe(catchError(() => of([])))
        .subscribe(list => {
          this.loading = false;
          this.buildMap(list);
          this.filterList(); // já aplica filtro
        });
    this.filterList();

  }

  // transforma lista plana em map agrupado por CNPJ
  private buildMap(list: ApiRecord[]) {
  const map: { [k: string]: CnpjRecord } = {};

  (list || []).forEach(item => {
    const digits = (item.cnpj || '').replace(/\D/g, '');
    const displayCnpj = this.formatCnpjMask(digits);
    const empresaName = (item.empresa || '').trim();

    // chave composta clara (sem whitespace problem) => encodeURIComponent evita issues
    const baseKey = `${digits}__${encodeURIComponent(empresaName)}`;

    const parcel: Parcel = {
      id: item.id,
      ddl: item.ddl,
      dueDate: item.data_pagamento,
      entryDate: item.data_entrada,
      value: Number(String(item.valor).replace(',', '.')) || 0
    };

    if (!map[baseKey]) {
      map[baseKey] = {
        displayCnpj: displayCnpj,
        razaoSocial: item.razaoSocial || '',
        empresa: empresaName || '',
        parcels: [parcel]
      } as CnpjRecord;
    } else {
      map[baseKey].parcels.push(parcel);
      if (!map[baseKey].empresa && item.empresa) map[baseKey].empresa = empresaName;
      if (!map[baseKey].razaoSocial && item.razaoSocial) map[baseKey].razaoSocial = item.razaoSocial;
    }
  });

  // ordenar parcels
  Object.keys(map).forEach(k => {
    map[k].parcels.sort((a,b) => new Date(a.dueDate || 0).getTime() - new Date(b.dueDate || 0).getTime());
  });

  this.recordsMap = map;

  const listas = Object.keys(map).map((k, idx) => ({
    _key: `${k}__${idx}`, 
    ...map[k]
  }));

  this.recordsListFull = listas; // lista completa sem filtro
  this.recordsList = [...listas]; 

  // transforma em lista e garante _key único (adiciona índice)
  // this.recordsList = Object.keys(map).map((k, idx) => ({
  //     originalKey: k,
  //     _key: `${k}__${idx}`,
  //     ...map[k]
  //   }));
}

// FILTRO POR CNPJ
filterList() {
  const term = (this.searchCnpj || '').replace(/\D/g, '');

  let entries = Object.entries(this.recordsMap); // [key, record]

  if (term.length > 0) {
    entries = entries.filter(([key]) => key.includes(term));
  }

  this.recordsList = entries.map(([key, record], idx) => ({
    originalKey: key,
    _key: `${key}__${idx}`,
    ...record
  }));
}

  toggle(key: string) {
  if (!key) {
    console.warn('toggle chamado sem key válida!', key);
    return;
  }

  this.expanded[key] = !this.expanded[key];
}


  // máscara simples para exibição
  formatCnpjMask(digits: string) {
    const d = (digits || '').padEnd(14, '0').slice(0,14);
    return d.replace(/^(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})$/, '$1.$2.$3/$4-$5');
  }

  // helpers para template
  formatBRL(v: number) {
    return v.toLocaleString('pt-BR', { style:'currency', currency:'BRL' });
  }

  formatDateBR(iso?: string) {
    if (!iso) return '-';
    const dt = new Date(iso);
    return `${String(dt.getDate()).padStart(2,'0')}/${String(dt.getMonth()+1).padStart(2,'0')}/${dt.getFullYear()}`;
  }

  // chamada manual para forçar recarregar (botão refresh)
  refresh() {
    this.loadAll();
  }
}

