import { STATUS_CFG } from "../constants/config";

export default function Badge({ status, forCitizen = false }) {
  const key = forCitizen && status === "Calculated" ? "Pending" : status;
  const s = STATUS_CFG[key] || STATUS_CFG.Pending;
  return (
    <span className={`inline-flex items-center gap-1.5 px-2.5 py-0.5 rounded-full text-xs font-bold tracking-wide border ${s.bg} ${s.color} ${s.border}`}>
      <span className={`w-1.5 h-1.5 rounded-full shrink-0 ${s.color.replace("text-", "bg-")}`} />
      {s.label}
    </span>
  );
}