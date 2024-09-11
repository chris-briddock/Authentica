interface SwitchProps {
  checked: boolean;
  onChange: () => void;
}

export const Switch: React.FC<SwitchProps> = ({ checked, onChange }) => (
  <label className="flex items-center cursor-pointer">
    <div className="relative">
      <input
        type="checkbox"
        className="sr-only"
        checked={checked}
        onChange={onChange}
      />
      <div
        className={`w-10 h-6 bg-gray-400 rounded-full shadow-inner ${
          checked ? "bg-blue-500" : ""
        }`}
      ></div>
      <div
        className={`absolute w-4 h-4 bg-white rounded-full shadow inset-y-1 left-1 transition-transform ${
          checked ? "transform translate-x-full" : ""
        }`}
      ></div>
    </div>
  </label>
);
