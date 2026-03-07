export default function Avatar({ name, size = 36 }) {
  const initials = name.split(" ").map(w => w[0]).slice(0, 2).join("");
  const hue = (name.charCodeAt(0) * 41 + name.charCodeAt(name.length - 1) * 19) % 360;
  return (
    <div
      className="rounded-full flex items-center justify-center font-extrabold shrink-0 select-none border-2"
      style={{
        width: size, height: size, fontSize: size * 0.33,
        background: `hsl(${hue},55%,90%)`,
        borderColor: `hsl(${hue},55%,78%)`,
        color: `hsl(${hue},55%,35%)`,
      }}
    >
      {initials}
    </div>
  );
}