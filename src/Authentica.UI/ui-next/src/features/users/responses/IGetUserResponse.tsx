export interface IGetUserResponse {
    id: string;
    userName: string;
    email: string;
    emailConfirmed: boolean;
    phoneNumber: string | null;
    phoneNumberConfirmed: boolean;
    twoFactorEnabled: boolean;
    lockoutEnd: string | null;
    lockoutEnabled: boolean;
    accessFailedCount: number;
    lastLoginDateTime: string | null;
    lastLoginIPAddress: string | null;
    isDeleted: boolean;
    deletedOnUtc: string | null;
    deletedBy: string | null;
    createdOnUtc: string;
    createdBy: string | null;
    modifiedOnUtc: string | null;
    modifiedBy: string | null;
    address: {
      name?: string;
      number?: string;
      street?: string;
      city?: string;
      state?: string;
      postcode?: string;
      country?: string;
    };
  }