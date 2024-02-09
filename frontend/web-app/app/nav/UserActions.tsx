"use client";

import { useSearchParamsStore } from "@/hooks/useSearchParamsStore";
import { Button, Dropdown } from "flowbite-react";
import { User } from "next-auth";
import { signOut } from "next-auth/react";
import Link from "next/link";
import { usePathname, useRouter } from "next/navigation";
import React from "react";
import { AiFillCar, AiFillTrophy, AiOutlineLogout } from "react-icons/ai";
import { HiCog, HiUser } from "react-icons/hi2";

type UserActionsProps = {
  user: User;
};

export default function UserActions({ user }: UserActionsProps) {
  const setParams = useSearchParamsStore(state => state.setParams);
  const router = useRouter();

  return (
    <Dropdown label={user.name} inline>
      <Dropdown.Item onClick={() => { setParams({seller: user.name ?? ''}); router.push('/') } } icon={HiUser}>
        My auctions
      </Dropdown.Item>
      <Dropdown.Item onClick={() => { setParams({winner: user.name ?? ''}); router.push('/') } }  icon={AiFillTrophy}>
        Auctions won
      </Dropdown.Item>
      <Dropdown.Item onClick={() => { router.push('/auctions/create') } }  icon={AiFillCar}>
        Sell my car
      </Dropdown.Item>
      <Dropdown.Item onClick={() => { router.push('/session') } }  icon={HiCog}>
        Session
      </Dropdown.Item>
      <Dropdown.Divider />
      <Dropdown.Item
        icon={AiOutlineLogout}
        onClick={() => {
          signOut({ callbackUrl: "/" });
        }}
      >
        Sign out
      </Dropdown.Item>
    </Dropdown>
  );
}
