import { useState } from 'react';
import styles from './CreateDeliveryForm.module.scss';
import {
  priorityOptions,
  productGroupOptions,
  type CreateDeliveryRequest,
} from '../../entities/delivery/types';

type Props = {
  onSubmit: (payload: CreateDeliveryRequest) => Promise<void>;
  onClose: () => void;
};

export const CreateDeliveryForm = ({ onSubmit, onClose }: Props) => {
  const [form, setForm] = useState({
    name: '',
    pickupNodeId: null as string | null,
    weightKg: 0,
    volumeM3: 0,
    productGroup: 1,
    windowStart: '',
    windowEnd: '',
    priority: 1,
  });

  const [submitting, setSubmitting] = useState(false);

  const handleChange = (field: string, value: any) => {
    setForm((prev) => ({
      ...prev,
      [field]: value,
    }));
  };

  const normalizeTime = (time?: string | null) => {
    if (!time) return null;
    return time.length === 5 ? `${time}:00` : time;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!form.name.trim()) return;

    try {
      setSubmitting(true);

      await onSubmit({
        name: form.name.trim(),
        pickupNodeId: form.pickupNodeId || null,
        weightKg: form.weightKg,
        volumeM3: form.volumeM3,
        productGroup: form.productGroup,
        windowStart: normalizeTime(form.windowStart),
        windowEnd: normalizeTime(form.windowEnd),
        priority: form.priority,
      });

      onClose();
    } catch (e) {
      console.error(e);
      alert('Не вдалося створити заявку');
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <div className={styles.overlay}>
      <div className={styles.modal}>
        <div className={styles.header}>
          <h3>Нова заявка</h3>
          <button type="button" onClick={onClose} className={styles.closeButton}>
            ×
          </button>
        </div>

        <form onSubmit={handleSubmit} className={styles.form}>
          <div className={styles.generated}>
            Номер заявки буде згенеровано автоматично
          </div>

          <label>
            <span>Адреса</span>
            <input
              value={form.name}
              onChange={(e) => handleChange('name', e.target.value)}
              placeholder="Наприклад: Київ, вул. Хрещатик 1"
              required
            />
          </label>

          <div className={styles.grid}>
            <label>
              <span>Вага, кг</span>
              <input
                type="number"
                step="0.01"
                min="0"
                value={form.weightKg}
                onChange={(e) => handleChange('weightKg', Number(e.target.value))}
                required
              />
            </label>

            <label>
              <span>Обʼєм, м³</span>
              <input
                type="number"
                step="0.01"
                min="0"
                value={form.volumeM3}
                onChange={(e) => handleChange('volumeM3', Number(e.target.value))}
                required
              />
            </label>
          </div>

          <div className={styles.grid}>
            <label>
              <span>Група товару</span>
              <select
                value={form.productGroup}
                onChange={(e) => handleChange('productGroup', Number(e.target.value))}
              >
                {productGroupOptions.map((option) => (
                  <option key={option.value} value={option.value}>
                    {option.label}
                  </option>
                ))}
              </select>
            </label>

            <label>
              <span>Пріоритет</span>
              <select
                value={form.priority}
                onChange={(e) => handleChange('priority', Number(e.target.value))}
              >
                {priorityOptions.map((option) => (
                  <option key={option.value} value={option.value}>
                    {option.label}
                  </option>
                ))}
              </select>
            </label>
          </div>

          <div className={styles.grid}>
            <label>
              <span>Вікно з</span>
              <input
                type="time"
                value={form.windowStart}
                onChange={(e) => handleChange('windowStart', e.target.value)}
              />
            </label>

            <label>
              <span>Вікно до</span>
              <input
                type="time"
                value={form.windowEnd}
                onChange={(e) => handleChange('windowEnd', e.target.value)}
              />
            </label>
          </div>

          <div className={styles.actions}>
            <button type="button" onClick={onClose} className={styles.secondaryButton}>
              Скасувати
            </button>

            <button type="submit" disabled={submitting} className={styles.primaryButton}>
              {submitting ? 'Створення...' : 'Створити'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};