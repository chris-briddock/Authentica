import { INavItem } from "@/common/models/INavItem";
import { Home, LogIn, UserPlus } from "lucide-react";

export const LoggedOutItems: INavItem[] = [
    { name: 'Home', icon: <Home className="h-5 w-5" />, href: '/' },
    { name: 'Login', icon: <LogIn className="h-5 w-5" />, href: '/login' },
    { name: 'Register', icon: <UserPlus className="h-5 w-5" />, href: '/register' },
];