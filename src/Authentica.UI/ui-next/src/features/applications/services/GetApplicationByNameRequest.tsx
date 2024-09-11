import { BASE_URL } from "@/common/constants/BaseUrl";

export const GetApplicationByNameRequest = async (name: string): Promise<Response> => {
    const URL = `${BASE_URL}/applications?Name=${encodeURIComponent(name)}`;
    const response = await fetch(URL, {
      method: 'GET',
      credentials: 'include',
      headers: {
        'Content-Type': 'application/json',
      },
    });
    return response;
  };