import styles from './StatCard.module.scss';

type Props = {
  title: string;
  value: number;
};

export const StatCard = ({ title, value }: Props) => {
  return (
    <div className={styles.card}>
      <div className={styles.title}>{title}</div>
      <div className={styles.value}>{value}</div>
    </div>
  );
};