import { baseApi } from '../../shared/api/baseApi';
import type {
  RouteListItemDto,
  RouteDetailsDto,
  BuildSingleRouteRequest,
  BuildManyRoutesRequest,
  BuildManyRoutesResponse,
} from './types';

type RoutesApiResult =
  | RouteListItemDto[]
  | { items?: RouteListItemDto[]; data?: RouteListItemDto[]; results?: RouteListItemDto[] };

const normalizeRoutes = (payload: RoutesApiResult): RouteListItemDto[] => {
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

export const getRoutes = async (): Promise<RouteListItemDto[]> => {
  const response = await baseApi.get<RoutesApiResult>('/routes');
  return normalizeRoutes(response.data);
};

export const getRouteById = async (id: string): Promise<RouteDetailsDto> => {
  const response = await baseApi.get<RouteDetailsDto>(`/routes/${id}`);
  return response.data;
};

export const buildSingle = async (
  payload: BuildSingleRouteRequest
): Promise<RouteDetailsDto> => {
  const response = await baseApi.post<RouteDetailsDto>('/routes/build', payload);
  return response.data;
};

export const buildMany = async (
  payload: BuildManyRoutesRequest
): Promise<BuildManyRoutesResponse> => {
  const response = await baseApi.post<BuildManyRoutesResponse>('/routes/build-many', payload);
  return response.data;
};

export const routeApi = {
  getRoutes,
  getRouteById,
  buildSingle,
  buildMany,
};