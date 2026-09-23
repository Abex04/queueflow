import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface RequestStatusChangedEvent {
  requestId: string;
  status: string;
  assignedTo: string | null;
}

@Injectable({ providedIn: 'root' })
export class RequestsHubService {
  private connection: signalR.HubConnection;
  public statusChanged$ = new Subject<RequestStatusChangedEvent>();

  constructor() {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}/hubs/requests`)
      .withAutomaticReconnect()
      .build();

    this.connection.on('RequestStatusChanged', (event: RequestStatusChangedEvent) => {
      this.statusChanged$.next(event);
    });
  }

  connect(): Promise<void> {
    return this.connection.start();
  }
}
