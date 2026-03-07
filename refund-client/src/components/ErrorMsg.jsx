export default function ErrorMsg({ message, onRetry }) {
  return (
    <div className="flex justify-between items-center bg-red-l border border-red-b rounded-xl px-4 py-3 text-red text-sm font-semibold">
      <span>⚠ {message}</span>
      {onRetry && (
        <button
          onClick={onRetry}
          className="text-red border border-red rounded-md px-3 py-1 text-xs bg-transparent hover:bg-red hover:text-white transition-colors"
        >
          נסה שוב
        </button>
      )}
    </div>
  );
}