import { NextResponse } from "next/server";

const API_POST_URL = (id: number) => `http://localhost:5138/api/v1/Posts/${id}`;

export async function GET(request: Request, id: number) {
  const requestOptions = {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
    },
  };

  const response = await fetch(API_POST_URL(id), requestOptions);
  const responseData = await response.json();

  return NextResponse.json({ ...responseData }, { status: 200 });
}
