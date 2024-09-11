import React from "react";
import { ReactNode } from "react";

interface TabsProps {
    children: ReactNode;
    activeTab: string;
    onTabChange: (tab: string) => void;
  }
  
 export const Tabs: React.FC<TabsProps> = ({ children, activeTab, onTabChange }) => (
    <div>
      <div className="flex mb-4 border-b border-gray-700">
        {React.Children.map(children, (child) => {
          if (React.isValidElement(child)) {
            return (
              <button
                className={`px-4 py-2 text-gray-300 hover:text-white ${activeTab === child.props.value ? 'border-b-2 border-blue-500 text-white' : ''}`}
                onClick={() => onTabChange(child.props.value)}
              >
                {child.props.label}
              </button>
            );
          }
          return null;
        })}
      </div>
      {React.Children.map(children, (child) => {
        if (React.isValidElement(child) && child.props.value === activeTab) {
          return child;
        }
        return null;
      })}
    </div>
  );
  