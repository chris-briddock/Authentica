import React from 'react';

interface ButtonProps {
  onClick: () => void;
  children: React.ReactNode;
  variant?: 'primary' | 'secondary' | 'danger';
  className?: string;
  disabled?: boolean;
  type?: 'button' | 'submit' | 'reset';
}

export const Button: React.FC<ButtonProps> = ({ 
  onClick, 
  children, 
  variant = 'primary', 
  className = '',
  disabled = false,
  type = 'button'
}) => {
  const baseStyles = 'px-4 py-2 rounded-md transition-colors duration-200';
  
  const variantStyles = {
    primary: 'bg-pink-500 text-white hover:bg-pink-600 focus:ring-pink-500',
    secondary: 'bg-gray-600 text-gray-300 hover:bg-gray-700 hover:text-white focus:ring-gray-500',
    danger: 'bg-red-500 text-white hover:bg-red-600 focus:ring-red-500'
  };

  const disabledStyles = 'opacity-50 cursor-not-allowed';

  return (
    <button
      onClick={onClick}
      className={`
        ${baseStyles}
        ${variantStyles[variant]}
        ${disabled ? disabledStyles : ''}
        focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-offset-gray-800
        ${className}
      `}
      disabled={disabled}
      type={type}
    >
      {children}
    </button>
  );
};

export default Button;