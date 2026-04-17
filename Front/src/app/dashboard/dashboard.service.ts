// ddl-cadastro.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { catchError, Observable, of } from 'rxjs';
import { environment } from 'environments/environment';

export interface ApiRecord {
  id: number;
  cnpj: string;
  empresa: string;
  razaoSocial?: string;
  valor: string | number; // API veio string
  ddl: number;
  data_pagamento: string; // ISO
  data_entrada: string;   // ISO
  data_pedido: string;
}

export interface Parcel {
  id?: number;
  ddl: number;
  dueDate: string;    // ISO date (data_pagamento)
  entryDate?: string; // ISO (data_entrada)
  value: number;
}

export interface CnpjRecord {
  displayCnpj: string;
  razaoSocial?: string;
  empresa?:string;
  parcels: Parcel[];
}

export interface CreateDto {
  cnpjempresa:string;
  empresa: string;
  cnpj: string;
  razaosocial: string;
  ddl: number[];
  valor: string;
}

@Injectable({ providedIn: 'root' })
export class DdlCadastroService {
  private apiUrl = environment.apiUrl;  

  constructor(private http: HttpClient) {}
  
  // GET all records
  getAll(): Observable<ApiRecord[]> {
    return this.http.get<ApiRecord[]>(`${this.apiUrl}/api/v1`);
  }

  searchByCnpj(cnpj: string): Observable<ApiRecord[]> {
    if (!cnpj) return this.getAll();
    const digits = cnpj.replace(/\D/g,'');
    const params = new HttpParams().set('cnpj', digits);
    // backend may ignore the param and return all; handle in caller
    return this.http.get<ApiRecord[]>(`${this.apiUrl}/api/v1`, { params }).pipe(
      catchError(() => of([]))
    );
  }

  // POST add parcel
  addParcel(payload: CreateDto) {
    return this.http.post(`${this.apiUrl}/api/v1`, payload);
  }

  // DELETE parcel by index (server will identify by CNPJ and parcel index)
  removeParcel(cnpj: string, parcelIndex: number) {
    // encode CNPJ in path
    return this.http.delete(`${this.apiUrl}/api/v1/${encodeURIComponent(cnpj)}/parcel/${parcelIndex}`);
  }

  // DELETE entire CNPJ record
  removeCnpj(cnpj: string) {
    return this.http.delete(`${this.apiUrl}/api/v1/${encodeURIComponent(cnpj)}`);
  }
}
