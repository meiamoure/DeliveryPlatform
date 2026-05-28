export type DeliveryPriority = 'Low' | 'Normal' | 'High';

export type DeliveryStatus =
  | 'Pending'
  | 'Planned'
  | 'InProgress'
  | 'Delivered'
  | 'Cancelled';

export type ProductGroup = 'Grocery' | 'Beverages' | 'HouseholdGoods' | 'Mixed';

export type DeliveryDto = {
  id: string;
  orderNumber: string;
  nodeId: string;
  nodeName: string;
  pickupNodeId?: string | null;
  weightKg: number;
  volumeM3: number;
  productGroup: string;
  serviceDate?: string | null;
  windowStart?: string | null;
  windowEnd?: string | null;
  priority: string;
  status: string;
};

export type CreateDeliveryRequest = {
  name: string;
  pickupNodeId?: string | null;
  weightKg: number;
  volumeM3: number;
  productGroup: number;
  windowStart?: string | null;
  windowEnd?: string | null;
  priority: number;
};

export type UpdateDeliveryRequest = {
  name: string;
  pickupNodeId?: string | null;
  load?: number | null;
  weightKg?: number | null;
  volumeM3?: number | null;
  productGroup?: number | null;
  windowStart?: string | null;
  windowEnd?: string | null;
  priority?: number | null;
};

export const productGroupOptions = [
  { value: 1, label: 'Продукти' },
  { value: 2, label: 'Напої' },
  { value: 3, label: 'Побутові товари' },
  { value: 4, label: 'Змішані' },
];

export const priorityOptions = [
  { value: 0, label: 'Низький' },
  { value: 1, label: 'Звичайний' },
  { value: 2, label: 'Високий' },
];

export const deliveryPriorityLabels: Record<DeliveryPriority, string> = {
  Low: 'Низький',
  Normal: 'Звичайний',
  High: 'Високий',
};

export const deliveryStatusLabels: Record<DeliveryStatus, string> = {
  Pending: 'Очікує',
  Planned: 'Запланована',
  InProgress: 'В процесі',
  Delivered: 'Доставлена',
  Cancelled: 'Скасована',
};

export const productGroupLabels: Record<ProductGroup, string> = {
  Grocery: 'Продукти',
  Beverages: 'Напої',
  HouseholdGoods: 'Побутові товари',
  Mixed: 'Змішані',
};