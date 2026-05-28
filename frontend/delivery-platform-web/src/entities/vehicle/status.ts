export type VehicleStatus = 'Available' | 'Busy' | 'Maintenance' | 'Disabled';

export const vehicleStatusLabels: Record<VehicleStatus, string> = {
  Available: 'Вільний',
  Busy: 'Зайнятий',
  Maintenance: 'На обслуговуванні',
  Disabled: 'Вимкнено',
};

export const vehicleStatusClassMap: Record<VehicleStatus, string> = {
  Available: 'available',
  Busy: 'busy',
  Maintenance: 'maintenance',
  Disabled: 'disabled',
};