import { useEffect, useRef, useState } from 'react';
import styles from './DeliveriesPage.module.scss';
import {
  cancelDelivery,
  createDelivery,
  getDeliveries,
  importDeliveriesCsv,
  updateDelivery,
} from '../entities/delivery/api';
import type { CreateDeliveryRequest, DeliveryDto } from '../entities/delivery/types';
import { DeliveriesTable } from '../widgets/DeliveriesTable';
import { CreateDeliveryForm } from '../features/delivery/CreateDeliveryForm';
import { UpdateDeliveryForm } from '../features/delivery/UpdateDeliveryForm';

export const DeliveriesPage = () => {
  const [deliveries, setDeliveries] = useState<DeliveryDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [isCreateOpen, setIsCreateOpen] = useState(false);
  const [editingDelivery, setEditingDelivery] = useState<DeliveryDto | null>(null);

  const fileInputRef = useRef<HTMLInputElement | null>(null);

  const loadDeliveries = async () => {
    try {
      setLoading(true);
      setError(null);

      const data = await getDeliveries();
      setDeliveries(data);
    } catch (e) {
      console.error(e);
      setError('Не вдалося загрузити заявки');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadDeliveries();
  }, []);

  const handleCreate = async (payload: CreateDeliveryRequest) => {
    await createDelivery(payload);
    await loadDeliveries();
  };

  const handleUpdate = async (payload: any) => {
    await updateDelivery(payload.id, payload);
    await loadDeliveries();
    setEditingDelivery(null);
  };

  const handleCancel = async (id: string) => {
    const confirmed = window.confirm('Відмінити цю заявку?');
    if (!confirmed) return;

    try {
      await cancelDelivery(id);
      await loadDeliveries();
    } catch (e) {
      console.error(e);
      alert('Не вдалося скасувати заявку');
    }
  };

  const handleImportClick = () => {
    fileInputRef.current?.click();
  };

  const handleFileSelected = async (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (!file) return;

    try {
      await importDeliveriesCsv(file);
      await loadDeliveries();
    } catch (err) {
      console.error(err);
      alert('Помилка при імпорті CSV');
    } finally {
      e.target.value = '';
    }
  };

  return (
    <div className={styles.page}>
      <div className={styles.top}>
        <h2 className={styles.title}>Заявки</h2>

        <div className={styles.topActions}>
          <input
            ref={fileInputRef}
            type="file"
            accept=".csv"
            onChange={handleFileSelected}
            className={styles.hiddenInput}
          />

          <button onClick={handleImportClick} className={styles.primaryButton}>
            Імпорт CSV
          </button>

          <button onClick={() => setIsCreateOpen(true)} className={styles.secondaryButton}>
            Нова заявка
          </button>
        </div>
      </div>

      {loading && <div className={styles.info}>Загрузка...</div>}
      {error && <div className={styles.error}>{error}</div>}

      {!loading && !error && (
        <DeliveriesTable
          deliveries={deliveries}
          onCancel={handleCancel}
          onEdit={(d) => setEditingDelivery(d)}
        />
      )}

      {isCreateOpen && (
        <CreateDeliveryForm
          onSubmit={handleCreate}
          onClose={() => setIsCreateOpen(false)}
        />
      )}

      {editingDelivery && (
        <UpdateDeliveryForm
          initialData={editingDelivery}
          onSubmit={handleUpdate}
          onClose={() => setEditingDelivery(null)}
        />
      )}
    </div>
  );
};