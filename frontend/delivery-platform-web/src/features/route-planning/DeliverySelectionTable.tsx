import type { DeliveryDto } from '../../entities/delivery/types';
import styles from './DeliverySelectionTable.module.scss';

type Props = {
  deliveries: DeliveryDto[];
  selectedDeliveryIds: string[];
  onToggle: (deliveryId: string) => void;
};

export const DeliverySelectionTable = ({
  deliveries,
  selectedDeliveryIds,
  onToggle,
}: Props) => {
  if (deliveries.length === 0) {
    return <div className={styles.empty}>Немає доступних заявок</div>;
  }

  return (
    <div className={styles.wrapper}>
      <table className={styles.table}>
        <thead>
          <tr>
            <th></th>
            <th>Номер</th>
            <th>Точка</th>
            <th>Вага</th>
            <th>Обʼєм</th>
            <th>Вікно</th>
            <th>Статус</th>
          </tr>
        </thead>

        <tbody>
          {deliveries.map((delivery) => {
            const isChecked = selectedDeliveryIds.includes(delivery.id);

            return (
              <tr key={delivery.id}>
                <td className={styles.checkboxCell}>
                  <input
                    type="checkbox"
                    checked={isChecked}
                    onChange={() => onToggle(delivery.id)}
                  />
                </td>
                <td>{delivery.orderNumber}</td>
                <td>{delivery.nodeName}</td>
                <td>{delivery.weightKg}</td>
                <td>{delivery.volumeM3}</td>
                <td>
                  {delivery.windowStart && delivery.windowEnd
                    ? `${delivery.windowStart.slice(0, 5)} - ${delivery.windowEnd.slice(0, 5)}`
                    : '—'}
                </td>
                <td>{delivery.status}</td>
              </tr>
            );
          })}
        </tbody>
      </table>
    </div>
  );
};