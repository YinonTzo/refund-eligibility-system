export default function CalcWarning({ message, onReject }) {
  return (
    <div className="bg-amber-l border border-amber-b rounded-xl p-4">
      <div className="flex items-start gap-2.5 mb-4">
        <span className="text-lg shrink-0">⚠️</span>
        <div>
          <div className="text-amber font-bold text-sm mb-1">לא ניתן לחשב זכאות</div>
          <div className="text-sub text-sm">{message}</div>
        </div>
      </div>
      <button
        onClick={onReject}
        className="w-full py-2.5 rounded-xl border border-red-b bg-red-l text-red font-bold text-sm hover:bg-red hover:text-white hover:border-red transition-colors"
      >
        ✕  דחה בקשה
      </button>
    </div>
  );
}