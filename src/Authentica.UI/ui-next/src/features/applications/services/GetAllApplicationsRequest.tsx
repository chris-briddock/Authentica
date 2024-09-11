import { BASE_URL } from "@/common/constants/BaseUrl";

export const GetAllApplicationsRequest = async (): Promise<Response> => {
    const URL = `${BASE_URL}/applications/all`;
    const response = await fetch(URL, {
      method: 'GET',
      credentials: 'include',
      headers: {
        'Content-Type': 'application/json',
      },
    });
    return response;
  };