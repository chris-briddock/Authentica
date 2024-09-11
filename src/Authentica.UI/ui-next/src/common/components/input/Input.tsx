import React, { ChangeEvent } from 'react';

interface InputProps {
  type?: 'text' | 'password' | 'email' | 'number' | 'tel' | 'url';
  value: string | number;
  onChange: (e: ChangeEvent<HTMLInputElement>) => void;
  placeholder?: string;
  className?: string;
  name?: string;
  id?: string;
  required?: boolean;
  disabled?: boolean;
}

export const Input: React.FC<InputProps> = ({ type = 'text', value, onChange, placeholder }) => (
  <input
    type={type}
    value={value}
    onChange={onChange}
    placeholder={placeholder}
    className="w-full px-3 py-2 bg-gray-600 text-white border border-gray-500 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
  />
);