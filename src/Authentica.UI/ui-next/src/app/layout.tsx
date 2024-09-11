import type { Metadata } from "next";
import "./globals.css";
import RecoilContextProvider from "@/common/components/RecoilContextProvider";

export const metadata: Metadata = {
  title: "Authentica",
  description: "An OAuth 2.0 authorization server.",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body>
        <RecoilContextProvider>{children}</RecoilContextProvider>
      </body>
    </html>
  );
}
