import type { Metadata } from "next";
import "./globals.css";
import { NavBar } from "./nav/Navbar";

export const metadata: Metadata = {
  title: "Microservices Test Project",
  description: "Created by me",
};

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="en">
      <body>
        <NavBar />
        <main className="container mx-auto px-5 pt-10">{children}</main>
      </body>
    </html>
  );
}
