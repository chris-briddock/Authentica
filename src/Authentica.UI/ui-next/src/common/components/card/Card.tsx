import React from 'react';

interface CardProps {
  title: string;
  children: React.ReactNode;
  className?: string;
}

export const Card: React.FC<CardProps> = ({ title, children }) => (
  <div className="bg-gray-700 shadow-md rounded-lg p-6 mb-6">
    <h2 className="text-xl font-semibold mb-4 text-white">{title}</h2>
    {children}
  </div>
);