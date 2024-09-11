"use client";

import { RecoilRoot } from "recoil";
import Footer from "./footer/Footer";
import CookiesPopup from "./cookie-popup/CookiePopup";
import { Navigation } from "@/features/navigation/components/Navigation";

export default function RecoilContextProvider({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <RecoilRoot>
      <Navigation />
      <CookiesPopup />
      <main>{children}</main>
      <Footer />
    </RecoilRoot>
  );
}
