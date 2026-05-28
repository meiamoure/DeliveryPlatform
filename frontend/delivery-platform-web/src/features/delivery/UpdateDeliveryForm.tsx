import { useState } from 'react';
import styles from './UpdateDeliveryForm.module.scss';
import {
  priorityOptions,
  productGroupOptions,
} from '../../entities/delivery/types';

type Props = {
  initialData: any;
  onSubmit: (payload: any) => Promise<void>;
  onClose: () => void;
};

export const UpdateDeliveryForm = ({ initialData, onSubmit, onClose }: Props) => {
  const [form, setForm] = useState({
    ...initialData,
    windowStart: initialData.windowStart?.slice(0, 5) ?? '',
    windowEnd: initialData.windowEnd?.slice(0, 5) ?? '',
  });

  const [submitting, setSubmitting] = useState(false);

  const handleChange = (field: string, value: any) => {
    setForm((prev: any) => ({
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

    try {
      setSubmitting(true);

      await onSubmit({
        ...form,
        windowStart: normalizeTime(form.windowStart),
        windowEnd: normalizeTime(form.windowEnd),
      });

      onClose();
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <div className={styles.overlay}>
      <div className={styles.modal}>
        {/* HEADER */}
        <div className={styles.header}>
          <h3>Редактировать заявку</h3>
          <button type="button" onClick={onClose} className={styles.closeButton}>
            ×
          </button>
        </div>

        <form onSubmit={handleSubmit} className={styles.form}>
          <label>
            <span>Номер заявки</span>
            <input
              value={form.orderNumber}
              onChange={(e) => handleChange('orderNumber', e.target.value)}
              required
            />
          </label>

          <label>
            <span>Адрес</span>
            <input
              value={form.name}
              onChange={(e) => handleChange('name', e.target.value)}
              required
            />
          </label>

          <div className={styles.grid}>
            <label>
              <span>Вес, кг</span>
              <input
                type="number"
                step="0.01"
                value={form.weightKg}
                onChange={(e) => handleChange('weightKg', Number(e.target.value))}
                required
              />
            </label>

            <label>
              <span>Объём, м³</span>
              <input
                type="number"
                step="0.01"
                value={form.volumeM3}
                onChange={(e) => handleChange('volumeM3', Number(e.target.value))}
                required
              />
            </label>
          </div>

          <div className={styles.grid}>
            <label>
              <span>Группа продукта</span>
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
              <span>Приоритет</span>
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
              <span>Окно с</span>
              <input
                type="time"
                value={form.windowStart ?? ''}
                onChange={(e) => handleChange('windowStart', e.target.value)}
              />
            </label>

            <label>
              <span>Окно до</span>
              <input
                type="time"
                value={form.windowEnd ?? ''}
                onChange={(e) => handleChange('windowEnd', e.target.value)}
              />
            </label>
          </div>

          <div className={styles.actions}>
            <button type="button" onClick={onClose} className={styles.secondaryButton}>
              Отмена
            </button>

            <button type="submit" disabled={submitting} className={styles.primaryButton}>
              {submitting ? 'Сохранение...' : 'Сохранить'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};