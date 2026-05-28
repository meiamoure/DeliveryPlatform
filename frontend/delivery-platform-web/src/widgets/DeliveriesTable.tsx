import styles from './DeliveriesTable.module.scss';
import type {
  DeliveryDto,
  DeliveryPriority,
  DeliveryStatus,
  ProductGroup,
} from '../entities/delivery/types';

type Props = {
  deliveries: DeliveryDto[];
  onCancel: (id: string) => void;
  onEdit: (delivery: DeliveryDto) => void;
};

const formatTime = (time?: string | null) => {
  if (!time) return '—';
  return time.slice(0, 5);
};

const formatWindow = (start?: string | null, end?: string | null) => {
  if (!start && !end) return '—';
  return `${formatTime(start)} - ${formatTime(end)}`;
};

const statusMap: Record<DeliveryStatus, string> = {
  Pending: 'В обробці',
  Planned: 'Запланована',
  InProgress: 'В процесі',
  Delivered: 'Доставлена',
  Cancelled: 'Скасована',
};

const priorityMap: Record<DeliveryPriority, string> = {
  Low: 'Низький',
  Normal: 'Звичайний',
  High: 'Високий',
};

const productGroupMap: Record<ProductGroup, string> = {
  Grocery: 'Продукти',
  Beverages: 'Напої',
  HouseholdGoods: 'Побутові товари',
  Mixed: 'Змішані',
};

export const DeliveriesTable = ({ deliveries, onCancel, onEdit }: Props) => {
  return (
    <div className={styles.wrapper}>
      <table className={styles.table}>
        <thead>
          <tr>
            <th>Номер</th>
            <th>Адреса</th>
            <th>Вага, кг</th>
            <th>Об'єм, м³</th>
            <th>Група</th>
            <th>Вікно</th>
            <th>Пріоритет</th>
            <th>Статус</th>
            <th>Дії</th>
          </tr>
        </thead>

        <tbody>
          {deliveries.map((delivery) => (
            <tr key={delivery.id} className={styles.row}>
              <td>{delivery.orderNumber}</td>
              <td>{delivery.nodeName}</td>
              <td>{delivery.weightKg}</td>
              <td>{delivery.volumeM3}</td>
              <td>{productGroupMap[delivery.productGroup as ProductGroup] ?? delivery.productGroup}</td>
              <td>{formatWindow(delivery.windowStart, delivery.windowEnd)}</td>
              <td className={styles.priority}>
                {priorityMap[delivery.priority as DeliveryPriority] ?? delivery.priority}
              </td>
              <td>
                <span
                  className={`${styles.status} ${styles['status' + delivery.status]}`}
                >
                  {statusMap[delivery.status as DeliveryStatus] ?? delivery.status}
                </span>
              </td>
              <td>
                <div className={styles.actions}>
                  <button
                    type="button"
                    className={styles.editButton}
                    onClick={() => onEdit(delivery)}
                  >
                    Ред.
                  </button>

                  <button
                    type="button"
                    className={styles.dangerButton}
                    onClick={() => onCancel(delivery.id)}
                    disabled={delivery.status === 'Cancelled'}
                  >
                    Скасування
                  </button>
                </div>
              </td>
            </tr>
          ))}

          {deliveries.length === 0 && (
            <tr>
              <td colSpan={9} className={styles.empty}>
                Заявки не найдены
              </td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
};