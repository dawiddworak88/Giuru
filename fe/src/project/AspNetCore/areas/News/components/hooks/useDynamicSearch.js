import { useEffect, useState, useContext } from "react";
import { Context } from "../../../../../../shared/stores/Store";
import QueryStringSerializer from "../../../../../../shared/helpers/serializers/QueryStringSerializer";

const useDynamicSearch = (apiUrl, newsList, itemsPerPage, pageIndex) => {
    const [state, dispatch] = useContext(Context);
    const [hasMore, setHasMore] = useState(false)
    const [news, setNews] = useState(newsList ? newsList : []);

    useEffect(() => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        const requestOptions = {
            method: "GET",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" }
        }

        const queryStrings = {
            itemsPerPage: itemsPerPage,
            pageIndex: pageIndex
        }

        const getNewsUrl = apiUrl + "?" + QueryStringSerializer.serialize(queryStrings);
        fetch(getNewsUrl, requestOptions)
            .then((response) => {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                return response.json().then(jsonResponse => {
                    if (response.ok){
                        setNews(jsonResponse.data)
                    }
                });
            });

    }, [itemsPerPage, pageIndex])

    return { news, hasMore };
}

export default useDynamicSearch;