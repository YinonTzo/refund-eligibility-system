export default function Spinner() {
  return (
    <div className="flex justify-center py-16">
      <div className="w-8 h-8 rounded-full border-[3px] border-border border-t-accent animate-spin" />
    </div>
  );
}