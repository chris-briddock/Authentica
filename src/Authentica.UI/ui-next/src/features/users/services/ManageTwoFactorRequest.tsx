import { BASE_URL } from "@/common/constants/BaseUrl";
import { IManageTwoFactorRequest } from "../requests/IManageTwoFactorRequest";

export const ManageTwoFactorRequest = async (request: IManageTwoFactorRequest ): Promise<Response> => {
    const URL = `${BASE_URL}/users/2fa/manage?is_enabled=${request.isEnabled}`;
    const response = await fetch(URL, {
      method: 'POST',
      credentials: 'include',
      headers: {
        'Content-Type': 'application/json',
      },
    });
    return response;
  };