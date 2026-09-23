import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import {
  CreateServiceRequestPayload,
  ServiceRequest,
  ServiceRequestDetail,
} from '../../../core/models/service-request.model';

@Injectable({ providedIn: 'root' })
export class RequestsService {
  private readonly baseUrl = `${environment.apiUrl}/api/requests`;

  constructor(private http: HttpClient) {}

  list(status?: string): Observable<ServiceRequest[]> {
    const url = status ? `${this.baseUrl}?status=${status}` : this.baseUrl;
    return this.http.get<ServiceRequest[]>(url);
  }

  getById(id: string): Observable<ServiceRequestDetail> {
    return this.http.get<ServiceRequestDetail>(`${this.baseUrl}/${id}`);
  }

  create(payload: CreateServiceRequestPayload): Observable<ServiceRequest> {
    return this.http.post<ServiceRequest>(this.baseUrl, payload);
  }

  changeStatus(id: string, newStatus: string, changedByUserId: string): Observable<ServiceRequest> {
    return this.http.patch<ServiceRequest>(`${this.baseUrl}/${id}/status`, { newStatus, changedByUserId });
  }

  assign(id: string, staffUserId: string): Observable<ServiceRequest> {
    return this.http.post<ServiceRequest>(`${this.baseUrl}/${id}/assign`, { staffUserId });
  }
}
