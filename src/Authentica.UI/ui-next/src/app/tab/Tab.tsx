import { ReactNode } from "react";

interface TabProps {
    value: string;
    label: string;
    children: ReactNode;
 }
  
 export const Tab: React.FC<TabProps> = ({ children }) => <div>{children}</div>;