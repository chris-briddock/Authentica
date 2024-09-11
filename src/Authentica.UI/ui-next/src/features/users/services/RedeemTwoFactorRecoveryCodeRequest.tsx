import { BASE_URL } from "@/common/constants/BaseUrl";
import { ITwoFactorRecoveryCodeRedeemRequest } from "../requests/ITwoFactorRecoveryCodeRedeemRequest";

export const RedeemTwoFactorRecoveryCodeRequest = async (data: ITwoFactorRecoveryCodeRedeemRequest): Promise<Response> => {
    const URL = `${BASE_URL}/users/2fa/recovery`;
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