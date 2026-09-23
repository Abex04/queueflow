export type RequestStatus = 'Waiting' | 'Assigned' | 'InProgress' | 'Completed' | 'Cancelled';

export interface ServiceRequest {
  id: string;
  title: string;
  description: string;
  serviceType: string;
  status: RequestStatus;
  assignedTo: string | null;
  createdAt: string;
  updatedAt: string;
}

export interface StatusHistoryEntry {
  fromStatus: RequestStatus;
  toStatus: RequestStatus;
  changedAt: string;
}

export interface ServiceRequestDetail extends ServiceRequest {
  statusHistory: StatusHistoryEntry[];
}

export interface CreateServiceRequestPayload {
  title: string;
  description: string;
  serviceType: string;
  createdByUserId: string;
}
