import { useEffect, useState } from 'react';
import { baseApi } from '../../shared/api/baseApi';

export type DriverRouteHistoryDto = {
  id: string;
  serviceDate: string;
  status: string;
  totalDistanceKm: number;
  totalTimeMin: number;
  number: number;
  code: string;
};

export const useDriverHistory = () => {
  const [data, setData] = useState<DriverRouteHistoryDto[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const load = async () => {
      try {
        const res = await baseApi.get('/driver/history');
        setData(res.data);
      } catch {
        setData([]);
      } finally {
        setLoading(false);
      }
    };

    load();
  }, []);

  return { data, loading };
};