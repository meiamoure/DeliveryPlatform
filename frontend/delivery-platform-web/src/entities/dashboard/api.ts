import { baseApi } from '../../shared/api/baseApi';
import type { DashboardStats } from './types';

export const getDashboardStats = async (date: string): Promise<DashboardStats> => {
  const response = await baseApi.get<DashboardStats>('/dashboard', {
    params: { date },
  });

  return response.data;
};