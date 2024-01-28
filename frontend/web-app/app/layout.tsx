import type { Metadata } from "next";
import "./globals.css";
import { NavBar } from "./nav/Navbar";
import ToasterProvider from "./providers/ToasterProvider";
import SignalRProvider from "./providers/SignalRProvider";
import { getCurrentUser } from "@/actions/authActions";
import { User } from "next-auth";

export const metadata: Metadata = {
  title: "Microservices Test Project",
  description: "Created by me",
};

export default async function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  const currentUser = await getCurrentUser();

  return (
    <html lang="en">
      <body>
        <ToasterProvider />
        <NavBar />
        <main className="container mx-auto px-5 pt-10">
          <SignalRProvider user={currentUser as User}>
            {children}
          </SignalRProvider>
        </main>
      </body>
    </html>
  );
}
