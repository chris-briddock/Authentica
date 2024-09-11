import { BASE_URL } from "@/common/constants/BaseUrl";

export const GenerateTwoFactorRecoveryCodesRequest = async (): Promise<Response> => {
    const URL = `${BASE_URL}/users/2fa/recovery/codes`;
    const response = await fetch(URL, {
      method: 'GET',
      credentials: 'include',
      headers: {
        'Content-Type': 'application/json',
      },
    });
    return response;
  };