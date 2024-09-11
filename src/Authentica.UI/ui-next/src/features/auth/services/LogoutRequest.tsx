import { BASE_URL } from "@/common/constants/BaseUrl";

export const LogoutRequest = async (): Promise<Response> => {
    const URL = `${BASE_URL}/users/logout`;
    const response = await fetch(URL, {
      method: 'POST',
      credentials: 'include',
      headers: {
        'Content-Type': 'application/json',
      },
    });
    return response;
  };