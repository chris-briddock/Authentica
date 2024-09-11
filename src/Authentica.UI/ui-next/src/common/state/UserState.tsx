import { atom } from "recoil";
import { IUser } from "../models/IUser";

export const userState = atom<IUser>({
    key: 'userState',
    default: {
      email: '',
      phoneNumber: '',
      address: '',
    },
  });