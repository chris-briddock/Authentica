import React from 'react';
import { CheckCircle, XCircle } from 'lucide-react';

interface AlertProps {
  type: 'success' | 'error';
  message: string;
}

const Alert: React.FC<AlertProps> = ({ type, message }) => {
  const bgColor = type === 'success' ? 'bg-green-500' : 'bg-red-500';
  const Icon = type === 'success' ? CheckCircle : XCircle;

  return (
    <div className={`${bgColor} text-white px-4 py-3 rounded relative`} role="alert">
      <div className="flex">
        <div className="py-1">
          <Icon className="h-6 w-6 text-white mr-4" />
        </div>
        <div>
          <p className="font-bold">{type === 'success' ? 'Success' : 'Error'}</p>
          <p className="text-sm">{message}</p>
        </div>
      </div>
    </div>
  );
};

export default Alert;