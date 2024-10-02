import { NextResponse } from "next/server";

const API_POSTS_URL = "http://localhost:5138/api/v1/Posts";

export async function GET(request: RequestInfo) {
  const requestOptions = {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
    },
  };

  const response = await fetch(API_POSTS_URL, requestOptions);
  const responseData = await response.json();

  return NextResponse.json({ ...responseData }, { status: 200 });
}

export async function POST(request: RequestInfo) {
  const requestOptions = {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Accept: "application/json",
    },
  };
}
