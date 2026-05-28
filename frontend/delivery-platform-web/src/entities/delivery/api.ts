import { baseApi } from '../../shared/api/baseApi';
import type {
  CreateDeliveryRequest,
  DeliveryDto,
  UpdateDeliveryRequest,
} from './types';

type DeliveriesApiResult =
  | DeliveryDto[]
  | { items?: DeliveryDto[]; data?: DeliveryDto[]; results?: DeliveryDto[] };

const normalizeDeliveries = (payload: DeliveriesApiResult): DeliveryDto[] => {
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

export const getDeliveries = async (
  page = 1,
  pageSize = 50
): Promise<DeliveryDto[]> => {
  const response = await baseApi.get<DeliveriesApiResult>('/deliveries', {
    params: { page, pageSize },
  });

  return normalizeDeliveries(response.data);
};

export const getPendingDeliveries = async (): Promise<DeliveryDto[]> => {
  const deliveries = await getDeliveries();
  return deliveries.filter((delivery) => delivery.status === 'Pending');
};

export const createDelivery = async (payload: CreateDeliveryRequest): Promise<void> => {
  await baseApi.post('/deliveries', payload);
};

export const updateDelivery = async (
  id: string,
  payload: UpdateDeliveryRequest
): Promise<void> => {
  await baseApi.put(`/deliveries/${id}`, payload);
};

export const cancelDelivery = async (id: string): Promise<void> => {
  await baseApi.post(`/deliveries/${id}/cancel`);
};

export const importDeliveriesCsv = async (file: File): Promise<void> => {
  const formData = new FormData();
  formData.append('file', file);

  await baseApi.post('/deliveries/import', formData, {
    headers: {
      'Content-Type': 'multipart/form-data',
    },
  });
};

export const deliveryApi = {
  getDeliveries,
  getPending: getPendingDeliveries,
  createDelivery,
  updateDelivery,
  cancelDelivery,
  importDeliveriesCsv,
};