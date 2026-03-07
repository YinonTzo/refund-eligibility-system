export default function Card({ children, className = "" }) {
  return (
    <div className={`bg-surf border border-border rounded-2xl shadow-sm ${className}`}>
      {children}
    </div>
  );
}