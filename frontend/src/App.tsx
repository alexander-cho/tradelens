import { useState } from 'react';
import './App.css';


const POSTS_API_URL = "http://localhost:5138/api/v1/Posts";


export default function App() {
    const [isDataFetched, setIsDataFetched] = useState(false);
    const [posts, setPosts] = useState<any[]>([]);

    async function getAllPosts() {
        try {
            const res = await fetch(POSTS_API_URL);
            const posts = await res.json();
            // console.log(posts);
            setPosts(posts);
        } catch (err) {
            console.error(err);
        }
    }

    function handleShowPostsClick() {
        setIsDataFetched(!isDataFetched);
        getAllPosts();
    }

    if (!isDataFetched) {
        return (
            <>
                <h1>TradeLens</h1>
                <button onClick={handleShowPostsClick}>Click here to retrieve all the posts.</button>
            </>
        )
    }

    return (
        <>
            <h1>TradeLens</h1>
            <ul>{posts.map((post, index) => (
                <li key={index}>{index+1}: {post.ticker}, {post.body}, {post.sentiment}</li>
            ))}</ul>
            <button>Click here to revert.</button>
        </>
    )
}
