import { INavItem } from "@/common/models/INavItem";
import { HelpCircle, Home, Settings, User } from "lucide-react";

export const LoggedInItems: INavItem[] = [
  { name: "Home", icon: <Home className="h-5 w-5" />, href: "/" },
  { name: "Profile", icon: <User className="h-5 w-5" />, href: "/profile" },
  {
    name: "Settings",
    icon: <Settings className="h-5 w-5" />,
    href: "/settings",
  },
  { name: "Help", icon: <HelpCircle className="h-5 w-5" />, href: "/help" },
];
