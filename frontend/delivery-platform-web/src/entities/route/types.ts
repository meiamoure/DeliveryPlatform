export type RouteListItemDto = {
  id: string;
  number: number;
  code: string; 
  vehicleId: string;
  vehiclePlate: string;
  driverId?: string | null;
  serviceDate: string;
  status: string;
  segmentsCount: number;
  totalDistanceKm: number;
  totalTimeMin: number;
};

export type RoutePointDto = {
  nodeId: string;
  lat: number;
  lng: number;
};

export type RouteSegmentDetailsDto = {
  order: number;
  fromNodeId: string;
  fromNodeName: string;
  toNodeId: string;
  toNodeName: string;
  distanceKm: number;
  travelTimeMin: number;
  deliveryId?: string | null;
};

export type RouteDetailsDto = {
  id: string;
  number: number;
  code: string;
  vehicleId: string;
  vehiclePlate: string;
  driverId?: string | null;
  driverName?: string | null;
  serviceDate: string;
  status: string;
  orderedPoints: RoutePointDto[];
  segments: RouteSegmentDetailsDto[];
  path: { lat: number; lng: number }[];
  stops: RouteStopDto[];
  totalDistanceKm: number;
  totalTimeMin: number;
};

export type BuildSingleRouteRequest = {
  depotNodeId: string;
  vehicleId: string;
  deliveryIds: string[];
  date: string;
};

export type BuildManyRoutesRequest = {
  depotNodeId: string;
  vehicleIds: string[];
  deliveryIds: string[];
  date: string;
};

export type BuildManyRoutesResponse = {
  routes: RouteDetailsDto[];
  unassignedDeliveryIds: string[];
};

export type RouteStopDto = {
  nodeId: string;
  nodeName: string;
  orderNumber?: string | null;
  lat: number;
  lng: number;
  sequence: number;
};

export const routeStatusLabels: Record<string, string> = {
  Planned: 'Заплановано',
  Accepted: 'Прийнято',
  InProgress: 'У процесі',
  Completed: 'Завершено',
  Cancelled: 'Скасовано',
};