import { BASE_URL } from "@/common/constants/BaseUrl";
import { ITwoFactorLoginRequest } from "../../users/requests/ITwoFactorLoginRequest";

export const TwoFactorLoginRequest = async (data: ITwoFactorLoginRequest): Promise<Response> => {
    const URL = `${BASE_URL}/users/2fa/login`;
    const response = await fetch(URL, {
      method: 'POST',
      credentials: 'include',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(data),
    });
    return response;
  };