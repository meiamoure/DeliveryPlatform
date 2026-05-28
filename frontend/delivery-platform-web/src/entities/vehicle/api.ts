import { baseApi } from '../../shared/api/baseApi';
import type { VehicleDto, SetVehicleStatusRequest, CreateVehicleRequest } from './types';

type VehiclesApiResult =
  | VehicleDto[]
  | { items?: VehicleDto[]; data?: VehicleDto[]; results?: VehicleDto[] };

const normalizeVehicles = (payload: VehiclesApiResult): VehicleDto[] => {
  if (Array.isArray(payload)) {
    return payload;
  }

  if (Array.isArray(payload.items)) {
    return payload.items;
  }

  if (Array.isArray(payload.data)) {
    return payload.data;
  }

  if (Array.isArray(payload.results)) {
    return payload.results;
  }

  return [];
};

export const getVehicles = async (): Promise<VehicleDto[]> => {
  const response = await baseApi.get<VehiclesApiResult>('/vehicles');
  return normalizeVehicles(response.data);
};

export const getAvailableVehicles = async (): Promise<VehicleDto[]> => {
  const vehicles = await getVehicles();
  return vehicles.filter((vehicle) => vehicle.status === 'Available');
};

export const createVehicle = async (payload: CreateVehicleRequest): Promise<void> => {
  await baseApi.post('/vehicles', payload);
};

export const setVehicleStatus = async (
  id: string,
  payload: SetVehicleStatusRequest
): Promise<void> => {
  await baseApi.patch(`/vehicles/${id}/status`, { status: payload.status });
};

export const unassignVehicle = async (id: string): Promise<void> => {
  await baseApi.patch(`/vehicles/${id}/unassign`);
};

export const vehicleApi = {
  getVehicles,
  getAvailable: getAvailableVehicles,
  createVehicle,
  setVehicleStatus,
  unassignVehicle,
};