import type { VehicleStatus } from './status';

export type VehicleDto = {
  id: string;
  plate: string;
  maxWeightKg: number;
  maxVolumeM3: number;
  speedKmh: number;
  status: VehicleStatus;
  depotNodeId: string;
  depotName?: string | null;
  currentRouteId?: string | null;
};

export type CreateVehicleRequest = {
  plate: string;
  maxWeightKg: number;
  maxVolumeM3: number;
  speedKmh: number;
  depotNodeId: string;
};

export type SetVehicleStatusRequest = {
  status: VehicleStatus;
};