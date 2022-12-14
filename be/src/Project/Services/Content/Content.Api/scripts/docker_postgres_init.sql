--
-- PostgreSQL database dump
--

-- Dumped from database version 10.5 (Debian 10.5-2.pgdg90+1)
-- Dumped by pg_dump version 15.0

-- Started on 2022-12-14 13:30:09

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 196 (class 1259 OID 16384)
-- Name: admin_permissions; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.admin_permissions (
    id integer NOT NULL,
    action character varying(255),
    subject character varying(255),
    properties jsonb,
    conditions jsonb,
    created_at timestamp(6) without time zone,
    updated_at timestamp(6) without time zone,
    created_by_id integer,
    updated_by_id integer
);


ALTER TABLE public.admin_permissions OWNER TO postgres;

--
-- TOC entry 197 (class 1259 OID 16390)
-- Name: admin_permissions_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.admin_permissions_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.admin_permissions_id_seq OWNER TO postgres;

--
-- TOC entry 3960 (class 0 OID 0)
-- Dependencies: 197
-- Name: admin_permissions_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.admin_permissions_id_seq OWNED BY public.admin_permissions.id;


--
-- TOC entry 198 (class 1259 OID 16392)
-- Name: admin_permissions_role_links; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.admin_permissions_role_links (
    id integer NOT NULL,
    permission_id integer,
    role_id integer,
    permission_order integer
);


ALTER TABLE public.admin_permissions_role_links OWNER TO postgres;

--
-- TOC entry 199 (class 1259 OID 16395)
-- Name: admin_permissions_role_links_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.admin_permissions_role_links_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.admin_permissions_role_links_id_seq OWNER TO postgres;

--
-- TOC entry 3961 (class 0 OID 0)
-- Dependencies: 199
-- Name: admin_permissions_role_links_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.admin_permissions_role_links_id_seq OWNED BY public.admin_permissions_role_links.id;


--
-- TOC entry 200 (class 1259 OID 16397)
-- Name: admin_roles; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.admin_roles (
    id integer NOT NULL,
    name character varying(255),
    code character varying(255),
    description character varying(255),
    created_at timestamp(6) without time zone,
    updated_at timestamp(6) without time zone,
    created_by_id integer,
    updated_by_id integer
);


ALTER TABLE public.admin_roles OWNER TO postgres;

--
-- TOC entry 201 (class 1259 OID 16403)
-- Name: admin_roles_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.admin_roles_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.admin_roles_id_seq OWNER TO postgres;

--
-- TOC entry 3962 (class 0 OID 0)
-- Dependencies: 201
-- Name: admin_roles_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.admin_roles_id_seq OWNED BY public.admin_roles.id;


--
-- TOC entry 202 (class 1259 OID 16405)
-- Name: admin_users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.admin_users (
    id integer NOT NULL,
    firstname character varying(255),
    lastname character varying(255),
    username character varying(255),
    email character varying(255),
    password character varying(255),
    reset_password_token character varying(255),
    registration_token character varying(255),
    is_active boolean,
    blocked boolean,
    prefered_language character varying(255),
    created_at timestamp(6) without time zone,
    updated_at timestamp(6) without time zone,
    created_by_id integer,
    updated_by_id integer
);


ALTER TABLE public.admin_users OWNER TO postgres;

--
-- TOC entry 203 (class 1259 OID 16411)
-- Name: admin_users_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.admin_users_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.admin_users_id_seq OWNER TO postgres;

--
-- TOC entry 3963 (class 0 OID 0)
-- Dependencies: 203
-- Name: admin_users_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.admin_users_id_seq OWNED BY public.admin_users.id;


--
-- TOC entry 204 (class 1259 OID 16413)
-- Name: admin_users_roles_links; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.admin_users_roles_links (
    id integer NOT NULL,
    user_id integer,
    role_id integer,
    role_order integer,
    user_order integer
);


ALTER TABLE public.admin_users_roles_links OWNER TO postgres;

--
-- TOC entry 205 (class 1259 OID 16416)
-- Name: admin_users_roles_links_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.admin_users_roles_links_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.admin_users_roles_links_id_seq OWNER TO postgres;

--
-- TOC entry 3964 (class 0 OID 0)
-- Dependencies: 205
-- Name: admin_users_roles_links_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.admin_users_roles_links_id_seq OWNED BY public.admin_users_roles_links.id;


--
-- TOC entry 297 (class 1259 OID 214095)
-- Name: available_products_pages; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.available_products_pages (
    id integer NOT NULL,
    created_at timestamp(6) without time zone,
    updated_at timestamp(6) without time zone,
    published_at timestamp(6) without time zone,
    created_by_id integer,
    updated_by_id integer,
    locale character varying(255)
);


ALTER TABLE public.available_products_pages OWNER TO postgres;

--
-- TOC entry 299 (class 1259 OID 214105)
-- Name: available_products_pages_components; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.available_products_pages_components (
    id integer NOT NULL,
    entity_id integer,
    component_id integer,
    component_type character varying(255),
    field character varying(255),
    "order" integer
);


ALTER TABLE public.available_products_pages_components OWNER TO postgres;

--
-- TOC entry 298 (class 1259 OID 214103)
-- Name: available_products_pages_components_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.available_products_pages_components_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.available_products_pages_components_id_seq OWNER TO postgres;

--
-- TOC entry 3965 (class 0 OID 0)
-- Dependencies: 298
-- Name: available_products_pages_components_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.available_products_pages_components_id_seq OWNED BY public.available_products_pages_components.id;


--
-- TOC entry 296 (class 1259 OID 214093)
-- Name: available_products_pages_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.available_products_pages_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.available_products_pages_id_seq OWNER TO postgres;

--
-- TOC entry 3966 (class 0 OID 0)
-- Dependencies: 296
-- Name: available_products_pages_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.available_products_pages_id_seq OWNED BY public.available_products_pages.id;


--
-- TOC entry 301 (class 1259 OID 214121)
-- Name: available_products_pages_localizations_links; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.available_products_pages_localizations_links (
    id integer NOT NULL,
    available_products_page_id integer,
    inv_available_products_page_id integer,
    available_products_page_order integer
);


ALTER TABLE public.available_products_pages_localizations_links OWNER TO postgres;

--
-- TOC entry 300 (class 1259 OID 214119)
-- Name: available_products_pages_localizations_links_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.available_products_pages_localizations_links_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.available_products_pages_localizations_links_id_seq OWNER TO postgres;

--
-- TOC entry 3967 (class 0 OID 0)
-- Dependencies: 300
-- Name: available_products_pages_localizations_links_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.available_products_pages_localizations_links_id_seq OWNED BY public.available_products_pages_localizations_links.id;


--
-- TOC entry 315 (class 1259 OID 214414)
-- Name: basket_pages; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.basket_pages (
    id integer NOT NULL,
    created_at timestamp(6) without time zone,
    updated_at timestamp(6) without time zone,
    published_at timestamp(6) without time zone,
    created_by_id integer,
    updated_by_id integer,
    locale character varying(255)
);


ALTER TABLE public.basket_pages OWNER TO postgres;

--
-- TOC entry 317 (class 1259 OID 214424)
-- Name: basket_pages_components; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.basket_pages_components (
    id integer NOT NULL,
    entity_id integer,
    component_id integer,
    component_type character varying(255),
    field character varying(255),
    "order" integer
);


ALTER TABLE public.basket_pages_components OWNER TO postgres;

--
-- TOC entry 316 (class 1259 OID 214422)
-- Name: basket_pages_components_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.basket_pages_components_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.basket_pages_components_id_seq OWNER TO postgres;

--
-- TOC entry 3968 (class 0 OID 0)
-- Dependencies: 316
-- Name: basket_pages_components_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.basket_pages_components_id_seq OWNED BY public.basket_pages_components.id;


--
-- TOC entry 314 (class 1259 OID 214412)
-- Name: basket_pages_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.basket_pages_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.basket_pages_id_seq OWNER TO postgres;

--
-- TOC entry 3969 (class 0 OID 0)
-- Dependencies: 314
-- Name: basket_pages_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.basket_pages_id_seq OWNED BY public.basket_pages.id;


--
-- TOC entry 319 (class 1259 OID 214440)
-- Name: basket_pages_localizations_links; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.basket_pages_localizations_links (
    id integer NOT NULL,
    basket_page_id integer,
    inv_basket_page_id integer,
    basket_page_order integer
);


ALTER TABLE public.basket_pages_localizations_links OWNER TO postgres;

--
-- TOC entry 318 (class 1259 OID 214438)
-- Name: basket_pages_localizations_links_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.basket_pages_localizations_links_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.basket_pages_localizations_links_id_seq OWNER TO postgres;

--
-- TOC entry 3970 (class 0 OID 0)
-- Dependencies: 318
-- Name: basket_pages_localizations_links_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.basket_pages_localizations_links_id_seq OWNED BY public.basket_pages_localizations_links.id;


--
-- TOC entry 206 (class 1259 OID 16418)
-- Name: components_blocks_hero_slider_items; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.components_blocks_hero_slider_items (
    id integer NOT NULL,
    title character varying(255),
    text character varying(255)
);


ALTER TABLE public.components_blocks_hero_slider_items OWNER TO postgres;

--
-- TOC entry 207 (class 1259 OID 16424)
-- Name: components_blocks_hero_slider_items_components; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.components_blocks_hero_slider_items_components (
    id integer NOT NULL,
    entity_id integer,
    component_id integer,
    component_type character varying(255),
    field character varying(255),
    "order" integer
);


ALTER TABLE public.components_blocks_hero_slider_items_components OWNER TO postgres;

--
-- TOC entry 208 (class 1259 OID 16430)
-- Name: components_blocks_hero_slider_items_components_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.components_blocks_hero_slider_items_components_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.components_blocks_hero_slider_items_components_id_seq OWNER TO postgres;

--
-- TOC entry 3971 (class 0 OID 0)
-- Dependencies: 208
-- Name: components_blocks_hero_slider_items_components_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.components_blocks_hero_slider_items_components_id_seq OWNED BY public.components_blocks_hero_slider_items_components.id;


--
-- TOC entry 209 (class 1259 OID 16432)
-- Name: components_blocks_hero_slider_items_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.components_blocks_hero_slider_items_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.components_blocks_hero_slider_items_id_seq OWNER TO postgres;

--
-- TOC entry 3972 (class 0 OID 0)
-- Dependencies: 209
-- Name: components_blocks_hero_slider_items_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.components_blocks_hero_slider_items_id_seq OWNED BY public.components_blocks_hero_slider_items.id;


--
-- TOC entry 210 (class 1259 OID 16434)
-- Name: components_blocks_hero_sliders; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.components_blocks_hero_sliders (
    id integer NOT NULL
);


ALTER TABLE public.components_blocks_hero_sliders OWNER TO postgres;

--
-- TOC entry 211 (class 1259 OID 16437)
-- Name: components_blocks_hero_sliders_components; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.components_blocks_hero_sliders_components (
    id integer NOT NULL,
    entity_id integer,
    component_id integer,
    component_type character varying(255),
    field character varying(255),
    "order" integer
);


ALTER TABLE public.components_blocks_hero_sliders_components OWNER TO postgres;

--
-- TOC entry 212 (class 1259 OID 16443)
-- Name: components_blocks_hero_sliders_components_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.components_blocks_hero_sliders_components_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.components_blocks_hero_sliders_components_id_seq OWNER TO postgres;

--
-- TOC entry 3973 (class 0 OID 0)
-- Dependencies: 212
-- Name: components_blocks_hero_sliders_components_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.components_blocks_hero_sliders_components_id_seq OWNED BY public.components_blocks_hero_sliders_components.id;


--
-- TOC entry 213 (class 1259 OID 16445)
-- Name: components_blocks_hero_sliders_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.components_blocks_hero_sliders_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.components_blocks_hero_sliders_id_seq OWNER TO postgres;

--
-- TOC entry 3974 (class 0 OID 0)
-- Dependencies: 213
-- Name: components_blocks_hero_sliders_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.components_blocks_hero_sliders_id_seq OWNED BY public.components_blocks_hero_sliders.id;


--
-- TOC entry 214 (class 1259 OID 16447)
-- Name: components_shared_links; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.components_shared_links (
    id integer NOT NULL,
    href character varying(255),
    label character varying(255),
    target character varying(255),
    is_external boolean
);


ALTER TABLE public.components_shared_links OWNER TO postgres;

--
-- TOC entry 215 (class 1259 OID 16453)
-- Name: components_shared_links_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.components_shared_links_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.components_shared_links_id_seq OWNER TO postgres;

--
-- TOC entry 3975 (class 0 OID 0)
-- Dependencies: 215
-- Name: components_shared_links_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.components_shared_links_id_seq OWNED BY public.components_shared_links.id;


--
-- TOC entry 216 (class 1259 OID 16455)
-- Name: components_shared_seos; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.components_shared_seos (
    id integer NOT NULL,
    meta_title character varying(255),
    meta_description character varying(255)
);


ALTER TABLE public.components_shared_seos OWNER TO postgres;

--
-- TOC entry 217 (class 1259 OID 16461)
-- Name: components_shared_seos_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.components_shared_seos_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.components_shared_seos_id_seq OWNER TO postgres;

--
-- TOC entry 3976 (class 0 OID 0)
-- Dependencies: 217
-- Name: components_shared_seos_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.components_shared_seos_id_seq OWNED BY public.components_shared_seos.id;


--
-- TOC entry 261 (class 1259 OID 213618)
-- Name: dashboard_pages; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.dashboard_pages (
    id integer NOT NULL,
    created_at timestamp(6) without time zone,
    updated_at timestamp(6) without time zone,
    published_at timestamp(6) without time zone,
    created_by_id integer,
    updated_by_id integer,
    locale character varying(255)
);


ALTER TABLE public.dashboard_pages OWNER TO postgres;

--
-- TOC entry 273 (class 1259 OID 213688)
-- Name: dashboard_pages_components; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.dashboard_pages_components (
    id integer NOT NULL,
    entity_id integer,
    component_id integer,
    component_type character varying(255),
    field character varying(255),
    "order" integer
);


ALTER TABLE public.dashboard_pages_components OWNER TO postgres;

--
-- TOC entry 272 (class 1259 OID 213686)
-- Name: dashboard_pages_components_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.dashboard_pages_components_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.dashboard_pages_components_id_seq OWNER TO postgres;

--
-- TOC entry 3977 (class 0 OID 0)
-- Dependencies: 272
-- Name: dashboard_pages_components_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.dashboard_pages_components_id_seq OWNED BY public.dashboard_pages_components.id;


--
-- TOC entry 260 (class 1259 OID 213616)
-- Name: dashboard_pages_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.dashboard_pages_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.dashboard_pages_id_seq OWNER TO postgres;

--
-- TOC entry 3978 (class 0 OID 0)
-- Dependencies: 260
-- Name: dashboard_pages_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.dashboard_pages_id_seq OWNED BY public.dashboard_pages.id;


--
-- TOC entry 275 (class 1259 OID 213704)
-- Name: dashboard_pages_localizations_links; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.dashboard_pages_localizations_links (
    id integer NOT NULL,
    dashboard_page_id integer,
    inv_dashboard_page_id integer,
    dashboard_page_order integer
);


ALTER TABLE public.dashboard_pages_localizations_links OWNER TO postgres;

--
-- TOC entry 274 (class 1259 OID 213702)
-- Name: dashboard_pages_localizations_links_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.dashboard_pages_localizations_links_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.dashboard_pages_localizations_links_id_seq OWNER TO postgres;

--
-- TOC entry 3979 (class 0 OID 0)
-- Dependencies: 274
-- Name: dashboard_pages_localizations_links_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.dashboard_pages_localizations_links_id_seq OWNED BY public.dashboard_pages_localizations_links.id;


--
-- TOC entry 263 (class 1259 OID 213628)
-- Name: download_center_pages; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.download_center_pages (
    id integer NOT NULL,
    created_at timestamp(6) without time zone,
    updated_at timestamp(6) without time zone,
    published_at timestamp(6) without time zone,
    created_by_id integer,
    updated_by_id integer,
    locale character varying(255)
);


ALTER TABLE public.download_center_pages OWNER TO postgres;

--
-- TOC entry 277 (class 1259 OID 213717)
-- Name: download_center_pages_components; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.download_center_pages_components (
    id integer NOT NULL,
    entity_id integer,
    component_id integer,
    component_type character varying(255),
    field character varying(255),
    "order" integer
);


ALTER TABLE public.download_center_pages_components OWNER TO postgres;

--
-- TOC entry 276 (class 1259 OID 213715)
-- Name: download_center_pages_components_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.download_center_pages_components_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.download_center_pages_components_id_seq OWNER TO postgres;

--
-- TOC entry 3980 (class 0 OID 0)
-- Dependencies: 276
-- Name: download_center_pages_components_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.download_center_pages_components_id_seq OWNED BY public.download_center_pages_components.id;


--
-- TOC entry 262 (class 1259 OID 213626)
-- Name: download_center_pages_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.download_center_pages_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.download_center_pages_id_seq OWNER TO postgres;

--
-- TOC entry 3981 (class 0 OID 0)
-- Dependencies: 262
-- Name: download_center_pages_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.download_center_pages_id_seq OWNED BY public.download_center_pages.id;


--
-- TOC entry 279 (class 1259 OID 213733)
-- Name: download_center_pages_localizations_links; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.download_center_pages_localizations_links (
    id integer NOT NULL,
    download_center_page_id integer,
    inv_download_center_page_id integer,
    download_center_page_order integer
);


ALTER TABLE public.download_center_pages_localizations_links OWNER TO postgres;

--
-- TOC entry 278 (class 1259 OID 213731)
-- Name: download_center_pages_localizations_links_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.download_center_pages_localizations_links_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.download_center_pages_localizations_links_id_seq OWNER TO postgres;

--
-- TOC entry 3982 (class 0 OID 0)
-- Dependencies: 278
-- Name: download_center_pages_localizations_links_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.download_center_pages_localizations_links_id_seq OWNED BY public.download_center_pages_localizations_links.id;


--
-- TOC entry 218 (class 1259 OID 16463)
-- Name: files; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.files (
    id integer NOT NULL,
    name character varying(255),
    alternative_text character varying(255),
    caption character varying(255),
    width integer,
    height integer,
    formats jsonb,
    hash character varying(255),
    ext character varying(255),
    mime character varying(255),
    size numeric(10,2),
    url character varying(255),
    preview_url character varying(255),
    provider character varying(255),
    provider_metadata jsonb,
    folder_path character varying(255),
    created_at timestamp(6) without time zone,
    updated_at timestamp(6) without time zone,
    created_by_id integer,
    updated_by_id integer
);


ALTER TABLE public.files OWNER TO postgres;

--
-- TOC entry 219 (class 1259 OID 16469)
-- Name: files_folder_links; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.files_folder_links (
    id integer NOT NULL,
    file_id integer,
    folder_id integer,
    file_order integer
);


ALTER TABLE public.files_folder_links OWNER TO postgres;

--
-- TOC entry 220 (class 1259 OID 16472)
-- Name: files_folder_links_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.files_folder_links_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.files_folder_links_id_seq OWNER TO postgres;

--
-- TOC entry 3983 (class 0 OID 0)
-- Dependencies: 220
-- Name: files_folder_links_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.files_folder_links_id_seq OWNED BY public.files_folder_links.id;


--
-- TOC entry 221 (class 1259 OID 16474)
-- Name: files_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.files_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.files_id_seq OWNER TO postgres;

--
-- TOC entry 3984 (class 0 OID 0)
-- Dependencies: 221
-- Name: files_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.files_id_seq OWNED BY public.files.id;


--
-- TOC entry 222 (class 1259 OID 16476)
-- Name: files_related_morphs; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.files_related_morphs (
    id integer NOT NULL,
    file_id integer,
    related_id integer,
    related_type character varying(255),
    field character varying(255),
    "order" integer
);


ALTER TABLE public.files_related_morphs OWNER TO postgres;

--
-- TOC entry 223 (class 1259 OID 16482)
-- Name: files_related_morphs_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.files_related_morphs_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.files_related_morphs_id_seq OWNER TO postgres;

--
-- TOC entry 3985 (class 0 OID 0)
-- Dependencies: 223
-- Name: files_related_morphs_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.files_related_morphs_id_seq OWNED BY public.files_related_morphs.id;


--
-- TOC entry 224 (class 1259 OID 16484)
-- Name: home_pages; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.home_pages (
    id integer NOT NULL,
    created_at timestamp(6) without time zone,
    updated_at timestamp(6) without time zone,
    published_at timestamp(6) without time zone,
    created_by_id integer,
    updated_by_id integer,
    locale character varying(255)
);


ALTER TABLE public.home_pages OWNER TO postgres;

--
-- TOC entry 225 (class 1259 OID 16487)
-- Name: home_pages_components; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.home_pages_components (
    id integer NOT NULL,
    entity_id integer,
    component_id integer,
    component_type character varying(255),
    field character varying(255),
    "order" integer
);


ALTER TABLE public.home_pages_components OWNER TO postgres;

--
-- TOC entry 226 (class 1259 OID 16493)
-- Name: home_pages_components_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.home_pages_components_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.home_pages_components_id_seq OWNER TO postgres;

--
-- TOC entry 3986 (class 0 OID 0)
-- Dependencies: 226
-- Name: home_pages_components_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.home_pages_components_id_seq OWNED BY public.home_pages_components.id;


--
-- TOC entry 227 (class 1259 OID 16495)
-- Name: home_pages_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.home_pages_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.home_pages_id_seq OWNER TO postgres;

--
-- TOC entry 3987 (class 0 OID 0)
-- Dependencies: 227
-- Name: home_pages_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.home_pages_id_seq OWNED BY public.home_pages.id;


--
-- TOC entry 228 (class 1259 OID 16497)
-- Name: home_pages_localizations_links; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.home_pages_localizations_links (
    id integer NOT NULL,
    home_page_id integer,
    inv_home_page_id integer,
    home_page_order integer
);


ALTER TABLE public.home_pages_localizations_links OWNER TO postgres;

--
-- TOC entry 229 (class 1259 OID 16500)
-- Name: home_pages_localizations_links_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.home_pages_localizations_links_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.home_pages_localizations_links_id_seq OWNER TO postgres;

--
-- TOC entry 3988 (class 0 OID 0)
-- Dependencies: 229
-- Name: home_pages_localizations_links_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.home_pages_localizations_links_id_seq OWNED BY public.home_pages_localizations_links.id;


--
-- TOC entry 230 (class 1259 OID 16502)
-- Name: i18n_locale; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.i18n_locale (
    id integer NOT NULL,
    name character varying(255),
    code character varying(255),
    created_at timestamp(6) without time zone,
    updated_at timestamp(6) without time zone,
    created_by_id integer,
    updated_by_id integer
);


ALTER TABLE public.i18n_locale OWNER TO postgres;

--
-- TOC entry 231 (class 1259 OID 16508)
-- Name: i18n_locale_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.i18n_locale_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.i18n_locale_id_seq OWNER TO postgres;

--
-- TOC entry 3989 (class 0 OID 0)
-- Dependencies: 231
-- Name: i18n_locale_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.i18n_locale_id_seq OWNED BY public.i18n_locale.id;


--
-- TOC entry 265 (class 1259 OID 213648)
-- Name: news_pages; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.news_pages (
    id integer NOT NULL,
    created_at timestamp(6) without time zone,
    updated_at timestamp(6) without time zone,
    published_at timestamp(6) without time zone,
    created_by_id integer,
    updated_by_id integer,
    locale character varying(255)
);


ALTER TABLE public.news_pages OWNER TO postgres;

--
-- TOC entry 281 (class 1259 OID 213775)
-- Name: news_pages_components; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.news_pages_components (
    id integer NOT NULL,
    entity_id integer,
    component_id integer,
    component_type character varying(255),
    field character varying(255),
    "order" integer
);


ALTER TABLE public.news_pages_components OWNER TO postgres;

--
-- TOC entry 280 (class 1259 OID 213773)
-- Name: news_pages_components_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.news_pages_components_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.news_pages_components_id_seq OWNER TO postgres;

--
-- TOC entry 3990 (class 0 OID 0)
-- Dependencies: 280
-- Name: news_pages_components_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.news_pages_components_id_seq OWNED BY public.news_pages_components.id;


--
-- TOC entry 264 (class 1259 OID 213646)
-- Name: news_pages_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.news_pages_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.news_pages_id_seq OWNER TO postgres;

--
-- TOC entry 3991 (class 0 OID 0)
-- Dependencies: 264
-- Name: news_pages_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.news_pages_id_seq OWNED BY public.news_pages.id;


--
-- TOC entry 283 (class 1259 OID 213791)
-- Name: news_pages_localizations_links; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.news_pages_localizations_links (
    id integer NOT NULL,
    news_page_id integer,
    inv_news_page_id integer,
    news_page_order integer
);


ALTER TABLE public.news_pages_localizations_links OWNER TO postgres;

--
-- TOC entry 282 (class 1259 OID 213789)
-- Name: news_pages_localizations_links_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.news_pages_localizations_links_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.news_pages_localizations_links_id_seq OWNER TO postgres;

--
-- TOC entry 3992 (class 0 OID 0)
-- Dependencies: 282
-- Name: news_pages_localizations_links_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.news_pages_localizations_links_id_seq OWNED BY public.news_pages_localizations_links.id;


--
-- TOC entry 321 (class 1259 OID 214505)
-- Name: order_item_pages; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.order_item_pages (
    id integer NOT NULL,
    created_at timestamp(6) without time zone,
    updated_at timestamp(6) without time zone,
    published_at timestamp(6) without time zone,
    created_by_id integer,
    updated_by_id integer,
    locale character varying(255)
);


ALTER TABLE public.order_item_pages OWNER TO postgres;

--
-- TOC entry 323 (class 1259 OID 214515)
-- Name: order_item_pages_components; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.order_item_pages_components (
    id integer NOT NULL,
    entity_id integer,
    component_id integer,
    component_type character varying(255),
    field character varying(255),
    "order" integer
);


ALTER TABLE public.order_item_pages_components OWNER TO postgres;

--
-- TOC entry 322 (class 1259 OID 214513)
-- Name: order_item_pages_components_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.order_item_pages_components_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.order_item_pages_components_id_seq OWNER TO postgres;

--
-- TOC entry 3993 (class 0 OID 0)
-- Dependencies: 322
-- Name: order_item_pages_components_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.order_item_pages_components_id_seq OWNED BY public.order_item_pages_components.id;


--
-- TOC entry 320 (class 1259 OID 214503)
-- Name: order_item_pages_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.order_item_pages_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.order_item_pages_id_seq OWNER TO postgres;

--
-- TOC entry 3994 (class 0 OID 0)
-- Dependencies: 320
-- Name: order_item_pages_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.order_item_pages_id_seq OWNED BY public.order_item_pages.id;


--
-- TOC entry 325 (class 1259 OID 214531)
-- Name: order_item_pages_localizations_links; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.order_item_pages_localizations_links (
    id integer NOT NULL,
    order_item_page_id integer,
    inv_order_item_page_id integer,
    order_item_page_order integer
);


ALTER TABLE public.order_item_pages_localizations_links OWNER TO postgres;

--
-- TOC entry 324 (class 1259 OID 214529)
-- Name: order_item_pages_localizations_links_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.order_item_pages_localizations_links_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.order_item_pages_localizations_links_id_seq OWNER TO postgres;

--
-- TOC entry 3995 (class 0 OID 0)
-- Dependencies: 324
-- Name: order_item_pages_localizations_links_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.order_item_pages_localizations_links_id_seq OWNED BY public.order_item_pages_localizations_links.id;


--
-- TOC entry 267 (class 1259 OID 213658)
-- Name: order_pages; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.order_pages (
    id integer NOT NULL,
    created_at timestamp(6) without time zone,
    updated_at timestamp(6) without time zone,
    published_at timestamp(6) without time zone,
    created_by_id integer,
    updated_by_id integer,
    locale character varying(255)
);


ALTER TABLE public.order_pages OWNER TO postgres;

--
-- TOC entry 285 (class 1259 OID 213804)
-- Name: order_pages_components; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.order_pages_components (
    id integer NOT NULL,
    entity_id integer,
    component_id integer,
    component_type character varying(255),
    field character varying(255),
    "order" integer
);


ALTER TABLE public.order_pages_components OWNER TO postgres;

--
-- TOC entry 284 (class 1259 OID 213802)
-- Name: order_pages_components_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.order_pages_components_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.order_pages_components_id_seq OWNER TO postgres;

--
-- TOC entry 3996 (class 0 OID 0)
-- Dependencies: 284
-- Name: order_pages_components_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.order_pages_components_id_seq OWNED BY public.order_pages_components.id;


--
-- TOC entry 266 (class 1259 OID 213656)
-- Name: order_pages_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.order_pages_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.order_pages_id_seq OWNER TO postgres;

--
-- TOC entry 3997 (class 0 OID 0)
-- Dependencies: 266
-- Name: order_pages_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.order_pages_id_seq OWNED BY public.order_pages.id;


--
-- TOC entry 287 (class 1259 OID 213820)
-- Name: order_pages_localizations_links; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.order_pages_localizations_links (
    id integer NOT NULL,
    order_page_id integer,
    inv_order_page_id integer,
    order_page_order integer
);


ALTER TABLE public.order_pages_localizations_links OWNER TO postgres;

--
-- TOC entry 286 (class 1259 OID 213818)
-- Name: order_pages_localizations_links_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.order_pages_localizations_links_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.order_pages_localizations_links_id_seq OWNER TO postgres;

--
-- TOC entry 3998 (class 0 OID 0)
-- Dependencies: 286
-- Name: order_pages_localizations_links_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.order_pages_localizations_links_id_seq OWNED BY public.order_pages_localizations_links.id;


--
-- TOC entry 269 (class 1259 OID 213668)
-- Name: orders_pages; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.orders_pages (
    id integer NOT NULL,
    created_at timestamp(6) without time zone,
    updated_at timestamp(6) without time zone,
    published_at timestamp(6) without time zone,
    created_by_id integer,
    updated_by_id integer,
    locale character varying(255)
);


ALTER TABLE public.orders_pages OWNER TO postgres;

--
-- TOC entry 289 (class 1259 OID 213833)
-- Name: orders_pages_components; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.orders_pages_components (
    id integer NOT NULL,
    entity_id integer,
    component_id integer,
    component_type character varying(255),
    field character varying(255),
    "order" integer
);


ALTER TABLE public.orders_pages_components OWNER TO postgres;

--
-- TOC entry 288 (class 1259 OID 213831)
-- Name: orders_pages_components_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.orders_pages_components_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.orders_pages_components_id_seq OWNER TO postgres;

--
-- TOC entry 3999 (class 0 OID 0)
-- Dependencies: 288
-- Name: orders_pages_components_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.orders_pages_components_id_seq OWNED BY public.orders_pages_components.id;


--
-- TOC entry 268 (class 1259 OID 213666)
-- Name: orders_pages_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.orders_pages_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.orders_pages_id_seq OWNER TO postgres;

--
-- TOC entry 4000 (class 0 OID 0)
-- Dependencies: 268
-- Name: orders_pages_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.orders_pages_id_seq OWNED BY public.orders_pages.id;


--
-- TOC entry 291 (class 1259 OID 213849)
-- Name: orders_pages_localizations_links; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.orders_pages_localizations_links (
    id integer NOT NULL,
    orders_page_id integer,
    inv_orders_page_id integer,
    orders_page_order integer
);


ALTER TABLE public.orders_pages_localizations_links OWNER TO postgres;

--
-- TOC entry 290 (class 1259 OID 213847)
-- Name: orders_pages_localizations_links_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.orders_pages_localizations_links_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.orders_pages_localizations_links_id_seq OWNER TO postgres;

--
-- TOC entry 4001 (class 0 OID 0)
-- Dependencies: 290
-- Name: orders_pages_localizations_links_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.orders_pages_localizations_links_id_seq OWNED BY public.orders_pages_localizations_links.id;


--
-- TOC entry 271 (class 1259 OID 213678)
-- Name: outlet_pages; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.outlet_pages (
    id integer NOT NULL,
    created_at timestamp(6) without time zone,
    updated_at timestamp(6) without time zone,
    published_at timestamp(6) without time zone,
    created_by_id integer,
    updated_by_id integer,
    locale character varying(255)
);


ALTER TABLE public.outlet_pages OWNER TO postgres;

--
-- TOC entry 293 (class 1259 OID 213862)
-- Name: outlet_pages_components; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.outlet_pages_components (
    id integer NOT NULL,
    entity_id integer,
    component_id integer,
    component_type character varying(255),
    field character varying(255),
    "order" integer
);


ALTER TABLE public.outlet_pages_components OWNER TO postgres;

--
-- TOC entry 292 (class 1259 OID 213860)
-- Name: outlet_pages_components_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.outlet_pages_components_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.outlet_pages_components_id_seq OWNER TO postgres;

--
-- TOC entry 4002 (class 0 OID 0)
-- Dependencies: 292
-- Name: outlet_pages_components_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.outlet_pages_components_id_seq OWNED BY public.outlet_pages_components.id;


--
-- TOC entry 270 (class 1259 OID 213676)
-- Name: outlet_pages_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.outlet_pages_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.outlet_pages_id_seq OWNER TO postgres;

--
-- TOC entry 4003 (class 0 OID 0)
-- Dependencies: 270
-- Name: outlet_pages_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.outlet_pages_id_seq OWNED BY public.outlet_pages.id;


--
-- TOC entry 295 (class 1259 OID 213878)
-- Name: outlet_pages_localizations_links; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.outlet_pages_localizations_links (
    id integer NOT NULL,
    outlet_page_id integer,
    inv_outlet_page_id integer,
    outlet_page_order integer
);


ALTER TABLE public.outlet_pages_localizations_links OWNER TO postgres;

--
-- TOC entry 294 (class 1259 OID 213876)
-- Name: outlet_pages_localizations_links_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.outlet_pages_localizations_links_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.outlet_pages_localizations_links_id_seq OWNER TO postgres;

--
-- TOC entry 4004 (class 0 OID 0)
-- Dependencies: 294
-- Name: outlet_pages_localizations_links_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.outlet_pages_localizations_links_id_seq OWNED BY public.outlet_pages_localizations_links.id;


--
-- TOC entry 309 (class 1259 OID 214271)
-- Name: privacy_policy_pages; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.privacy_policy_pages (
    id integer NOT NULL,
    created_at timestamp(6) without time zone,
    updated_at timestamp(6) without time zone,
    published_at timestamp(6) without time zone,
    created_by_id integer,
    updated_by_id integer,
    locale character varying(255),
    text text
);


ALTER TABLE public.privacy_policy_pages OWNER TO postgres;

--
-- TOC entry 311 (class 1259 OID 214284)
-- Name: privacy_policy_pages_components; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.privacy_policy_pages_components (
    id integer NOT NULL,
    entity_id integer,
    component_id integer,
    component_type character varying(255),
    field character varying(255),
    "order" integer
);


ALTER TABLE public.privacy_policy_pages_components OWNER TO postgres;

--
-- TOC entry 310 (class 1259 OID 214282)
-- Name: privacy_policy_pages_components_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.privacy_policy_pages_components_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.privacy_policy_pages_components_id_seq OWNER TO postgres;

--
-- TOC entry 4005 (class 0 OID 0)
-- Dependencies: 310
-- Name: privacy_policy_pages_components_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.privacy_policy_pages_components_id_seq OWNED BY public.privacy_policy_pages_components.id;


--
-- TOC entry 308 (class 1259 OID 214269)
-- Name: privacy_policy_pages_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.privacy_policy_pages_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.privacy_policy_pages_id_seq OWNER TO postgres;

--
-- TOC entry 4006 (class 0 OID 0)
-- Dependencies: 308
-- Name: privacy_policy_pages_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.privacy_policy_pages_id_seq OWNED BY public.privacy_policy_pages.id;


--
-- TOC entry 313 (class 1259 OID 214300)
-- Name: privacy_policy_pages_localizations_links; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.privacy_policy_pages_localizations_links (
    id integer NOT NULL,
    privacy_policy_page_id integer,
    inv_privacy_policy_page_id integer,
    privacy_policy_page_order integer
);


ALTER TABLE public.privacy_policy_pages_localizations_links OWNER TO postgres;

--
-- TOC entry 312 (class 1259 OID 214298)
-- Name: privacy_policy_pages_localizations_links_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.privacy_policy_pages_localizations_links_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.privacy_policy_pages_localizations_links_id_seq OWNER TO postgres;

--
-- TOC entry 4007 (class 0 OID 0)
-- Dependencies: 312
-- Name: privacy_policy_pages_localizations_links_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.privacy_policy_pages_localizations_links_id_seq OWNED BY public.privacy_policy_pages_localizations_links.id;


--
-- TOC entry 303 (class 1259 OID 214181)
-- Name: regulations_pages; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.regulations_pages (
    id integer NOT NULL,
    created_at timestamp(6) without time zone,
    updated_at timestamp(6) without time zone,
    published_at timestamp(6) without time zone,
    created_by_id integer,
    updated_by_id integer,
    locale character varying(255),
    text text
);


ALTER TABLE public.regulations_pages OWNER TO postgres;

--
-- TOC entry 305 (class 1259 OID 214194)
-- Name: regulations_pages_components; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.regulations_pages_components (
    id integer NOT NULL,
    entity_id integer,
    component_id integer,
    component_type character varying(255),
    field character varying(255),
    "order" integer
);


ALTER TABLE public.regulations_pages_components OWNER TO postgres;

--
-- TOC entry 304 (class 1259 OID 214192)
-- Name: regulations_pages_components_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.regulations_pages_components_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.regulations_pages_components_id_seq OWNER TO postgres;

--
-- TOC entry 4008 (class 0 OID 0)
-- Dependencies: 304
-- Name: regulations_pages_components_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.regulations_pages_components_id_seq OWNED BY public.regulations_pages_components.id;


--
-- TOC entry 302 (class 1259 OID 214179)
-- Name: regulations_pages_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.regulations_pages_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.regulations_pages_id_seq OWNER TO postgres;

--
-- TOC entry 4009 (class 0 OID 0)
-- Dependencies: 302
-- Name: regulations_pages_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.regulations_pages_id_seq OWNED BY public.regulations_pages.id;


--
-- TOC entry 307 (class 1259 OID 214210)
-- Name: regulations_pages_localizations_links; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.regulations_pages_localizations_links (
    id integer NOT NULL,
    regulations_page_id integer,
    inv_regulations_page_id integer,
    regulations_page_order integer
);


ALTER TABLE public.regulations_pages_localizations_links OWNER TO postgres;

--
-- TOC entry 306 (class 1259 OID 214208)
-- Name: regulations_pages_localizations_links_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.regulations_pages_localizations_links_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.regulations_pages_localizations_links_id_seq OWNER TO postgres;

--
-- TOC entry 4010 (class 0 OID 0)
-- Dependencies: 306
-- Name: regulations_pages_localizations_links_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.regulations_pages_localizations_links_id_seq OWNED BY public.regulations_pages_localizations_links.id;


--
-- TOC entry 327 (class 1259 OID 214597)
-- Name: search_products_pages; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.search_products_pages (
    id integer NOT NULL,
    created_at timestamp(6) without time zone,
    updated_at timestamp(6) without time zone,
    published_at timestamp(6) without time zone,
    created_by_id integer,
    updated_by_id integer,
    locale character varying(255)
);


ALTER TABLE public.search_products_pages OWNER TO postgres;

--
-- TOC entry 329 (class 1259 OID 214607)
-- Name: search_products_pages_components; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.search_products_pages_components (
    id integer NOT NULL,
    entity_id integer,
    component_id integer,
    component_type character varying(255),
    field character varying(255),
    "order" integer
);


ALTER TABLE public.search_products_pages_components OWNER TO postgres;

--
-- TOC entry 328 (class 1259 OID 214605)
-- Name: search_products_pages_components_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.search_products_pages_components_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.search_products_pages_components_id_seq OWNER TO postgres;

--
-- TOC entry 4011 (class 0 OID 0)
-- Dependencies: 328
-- Name: search_products_pages_components_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.search_products_pages_components_id_seq OWNED BY public.search_products_pages_components.id;


--
-- TOC entry 326 (class 1259 OID 214595)
-- Name: search_products_pages_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.search_products_pages_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.search_products_pages_id_seq OWNER TO postgres;

--
-- TOC entry 4012 (class 0 OID 0)
-- Dependencies: 326
-- Name: search_products_pages_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.search_products_pages_id_seq OWNED BY public.search_products_pages.id;


--
-- TOC entry 331 (class 1259 OID 214623)
-- Name: search_products_pages_localizations_links; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.search_products_pages_localizations_links (
    id integer NOT NULL,
    search_products_page_id integer,
    inv_search_products_page_id integer,
    search_products_page_order integer
);


ALTER TABLE public.search_products_pages_localizations_links OWNER TO postgres;

--
-- TOC entry 330 (class 1259 OID 214621)
-- Name: search_products_pages_localizations_links_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.search_products_pages_localizations_links_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.search_products_pages_localizations_links_id_seq OWNER TO postgres;

--
-- TOC entry 4013 (class 0 OID 0)
-- Dependencies: 330
-- Name: search_products_pages_localizations_links_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.search_products_pages_localizations_links_id_seq OWNED BY public.search_products_pages_localizations_links.id;


--
-- TOC entry 232 (class 1259 OID 16510)
-- Name: strapi_api_token_permissions; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.strapi_api_token_permissions (
    id integer NOT NULL,
    action character varying(255),
    created_at timestamp(6) without time zone,
    updated_at timestamp(6) without time zone,
    created_by_id integer,
    updated_by_id integer
);


ALTER TABLE public.strapi_api_token_permissions OWNER TO postgres;

--
-- TOC entry 233 (class 1259 OID 16513)
-- Name: strapi_api_token_permissions_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.strapi_api_token_permissions_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.strapi_api_token_permissions_id_seq OWNER TO postgres;

--
-- TOC entry 4014 (class 0 OID 0)
-- Dependencies: 233
-- Name: strapi_api_token_permissions_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.strapi_api_token_permissions_id_seq OWNED BY public.strapi_api_token_permissions.id;


--
-- TOC entry 234 (class 1259 OID 16515)
-- Name: strapi_api_token_permissions_token_links; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.strapi_api_token_permissions_token_links (
    id integer NOT NULL,
    api_token_permission_id integer,
    api_token_id integer,
    api_token_permission_order integer
);


ALTER TABLE public.strapi_api_token_permissions_token_links OWNER TO postgres;

--
-- TOC entry 235 (class 1259 OID 16518)
-- Name: strapi_api_token_permissions_token_links_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.strapi_api_token_permissions_token_links_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.strapi_api_token_permissions_token_links_id_seq OWNER TO postgres;

--
-- TOC entry 4015 (class 0 OID 0)
-- Dependencies: 235
-- Name: strapi_api_token_permissions_token_links_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.strapi_api_token_permissions_token_links_id_seq OWNED BY public.strapi_api_token_permissions_token_links.id;


--
-- TOC entry 236 (class 1259 OID 16520)
-- Name: strapi_api_tokens; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.strapi_api_tokens (
    id integer NOT NULL,
    name character varying(255),
    description character varying(255),
    type character varying(255),
    access_key character varying(255),
    last_used_at timestamp(6) without time zone,
    expires_at timestamp(6) without time zone,
    lifespan bigint,
    created_at timestamp(6) without time zone,
    updated_at timestamp(6) without time zone,
    created_by_id integer,
    updated_by_id integer
);


ALTER TABLE public.strapi_api_tokens OWNER TO postgres;

--
-- TOC entry 237 (class 1259 OID 16526)
-- Name: strapi_api_tokens_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.strapi_api_tokens_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.strapi_api_tokens_id_seq OWNER TO postgres;

--
-- TOC entry 4016 (class 0 OID 0)
-- Dependencies: 237
-- Name: strapi_api_tokens_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.strapi_api_tokens_id_seq OWNED BY public.strapi_api_tokens.id;


--
-- TOC entry 238 (class 1259 OID 16528)
-- Name: strapi_core_store_settings; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.strapi_core_store_settings (
    id integer NOT NULL,
    key character varying(255),
    value text,
    type character varying(255),
    environment character varying(255),
    tag character varying(255)
);


ALTER TABLE public.strapi_core_store_settings OWNER TO postgres;

--
-- TOC entry 239 (class 1259 OID 16534)
-- Name: strapi_core_store_settings_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.strapi_core_store_settings_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.strapi_core_store_settings_id_seq OWNER TO postgres;

--
-- TOC entry 4017 (class 0 OID 0)
-- Dependencies: 239
-- Name: strapi_core_store_settings_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.strapi_core_store_settings_id_seq OWNED BY public.strapi_core_store_settings.id;


--
-- TOC entry 240 (class 1259 OID 16536)
-- Name: strapi_database_schema; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.strapi_database_schema (
    id integer NOT NULL,
    schema json,
    "time" timestamp without time zone,
    hash character varying(255)
);


ALTER TABLE public.strapi_database_schema OWNER TO postgres;

--
-- TOC entry 241 (class 1259 OID 16542)
-- Name: strapi_database_schema_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.strapi_database_schema_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.strapi_database_schema_id_seq OWNER TO postgres;

--
-- TOC entry 4018 (class 0 OID 0)
-- Dependencies: 241
-- Name: strapi_database_schema_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.strapi_database_schema_id_seq OWNED BY public.strapi_database_schema.id;


--
-- TOC entry 242 (class 1259 OID 16544)
-- Name: strapi_migrations; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.strapi_migrations (
    id integer NOT NULL,
    name character varying(255),
    "time" timestamp without time zone
);


ALTER TABLE public.strapi_migrations OWNER TO postgres;

--
-- TOC entry 243 (class 1259 OID 16547)
-- Name: strapi_migrations_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.strapi_migrations_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.strapi_migrations_id_seq OWNER TO postgres;

--
-- TOC entry 4019 (class 0 OID 0)
-- Dependencies: 243
-- Name: strapi_migrations_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.strapi_migrations_id_seq OWNED BY public.strapi_migrations.id;


--
-- TOC entry 244 (class 1259 OID 16549)
-- Name: strapi_webhooks; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.strapi_webhooks (
    id integer NOT NULL,
    name character varying(255),
    url text,
    headers jsonb,
    events jsonb,
    enabled boolean
);


ALTER TABLE public.strapi_webhooks OWNER TO postgres;

--
-- TOC entry 245 (class 1259 OID 16555)
-- Name: strapi_webhooks_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.strapi_webhooks_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.strapi_webhooks_id_seq OWNER TO postgres;

--
-- TOC entry 4020 (class 0 OID 0)
-- Dependencies: 245
-- Name: strapi_webhooks_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.strapi_webhooks_id_seq OWNED BY public.strapi_webhooks.id;


--
-- TOC entry 246 (class 1259 OID 16557)
-- Name: up_permissions; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.up_permissions (
    id integer NOT NULL,
    action character varying(255),
    created_at timestamp(6) without time zone,
    updated_at timestamp(6) without time zone,
    created_by_id integer,
    updated_by_id integer
);


ALTER TABLE public.up_permissions OWNER TO postgres;

--
-- TOC entry 247 (class 1259 OID 16560)
-- Name: up_permissions_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.up_permissions_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.up_permissions_id_seq OWNER TO postgres;

--
-- TOC entry 4021 (class 0 OID 0)
-- Dependencies: 247
-- Name: up_permissions_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.up_permissions_id_seq OWNED BY public.up_permissions.id;


--
-- TOC entry 248 (class 1259 OID 16562)
-- Name: up_permissions_role_links; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.up_permissions_role_links (
    id integer NOT NULL,
    permission_id integer,
    role_id integer,
    permission_order integer
);


ALTER TABLE public.up_permissions_role_links OWNER TO postgres;

--
-- TOC entry 249 (class 1259 OID 16565)
-- Name: up_permissions_role_links_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.up_permissions_role_links_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.up_permissions_role_links_id_seq OWNER TO postgres;

--
-- TOC entry 4022 (class 0 OID 0)
-- Dependencies: 249
-- Name: up_permissions_role_links_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.up_permissions_role_links_id_seq OWNED BY public.up_permissions_role_links.id;


--
-- TOC entry 250 (class 1259 OID 16567)
-- Name: up_roles; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.up_roles (
    id integer NOT NULL,
    name character varying(255),
    description character varying(255),
    type character varying(255),
    created_at timestamp(6) without time zone,
    updated_at timestamp(6) without time zone,
    created_by_id integer,
    updated_by_id integer
);


ALTER TABLE public.up_roles OWNER TO postgres;

--
-- TOC entry 251 (class 1259 OID 16573)
-- Name: up_roles_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.up_roles_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.up_roles_id_seq OWNER TO postgres;

--
-- TOC entry 4023 (class 0 OID 0)
-- Dependencies: 251
-- Name: up_roles_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.up_roles_id_seq OWNED BY public.up_roles.id;


--
-- TOC entry 252 (class 1259 OID 16575)
-- Name: up_users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.up_users (
    id integer NOT NULL,
    username character varying(255),
    email character varying(255),
    provider character varying(255),
    password character varying(255),
    reset_password_token character varying(255),
    confirmation_token character varying(255),
    confirmed boolean,
    blocked boolean,
    created_at timestamp(6) without time zone,
    updated_at timestamp(6) without time zone,
    created_by_id integer,
    updated_by_id integer
);


ALTER TABLE public.up_users OWNER TO postgres;

--
-- TOC entry 253 (class 1259 OID 16581)
-- Name: up_users_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.up_users_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.up_users_id_seq OWNER TO postgres;

--
-- TOC entry 4024 (class 0 OID 0)
-- Dependencies: 253
-- Name: up_users_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.up_users_id_seq OWNED BY public.up_users.id;


--
-- TOC entry 254 (class 1259 OID 16583)
-- Name: up_users_role_links; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.up_users_role_links (
    id integer NOT NULL,
    user_id integer,
    role_id integer,
    user_order integer
);


ALTER TABLE public.up_users_role_links OWNER TO postgres;

--
-- TOC entry 255 (class 1259 OID 16586)
-- Name: up_users_role_links_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.up_users_role_links_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.up_users_role_links_id_seq OWNER TO postgres;

--
-- TOC entry 4025 (class 0 OID 0)
-- Dependencies: 255
-- Name: up_users_role_links_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.up_users_role_links_id_seq OWNED BY public.up_users_role_links.id;


--
-- TOC entry 256 (class 1259 OID 16589)
-- Name: upload_folders; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.upload_folders (
    id integer NOT NULL,
    name character varying(255),
    path_id integer,
    path character varying(255),
    created_at timestamp(6) without time zone,
    updated_at timestamp(6) without time zone,
    created_by_id integer,
    updated_by_id integer
);


ALTER TABLE public.upload_folders OWNER TO postgres;

--
-- TOC entry 257 (class 1259 OID 16595)
-- Name: upload_folders_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.upload_folders_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.upload_folders_id_seq OWNER TO postgres;

--
-- TOC entry 4026 (class 0 OID 0)
-- Dependencies: 257
-- Name: upload_folders_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.upload_folders_id_seq OWNED BY public.upload_folders.id;


--
-- TOC entry 258 (class 1259 OID 16597)
-- Name: upload_folders_parent_links; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.upload_folders_parent_links (
    id integer NOT NULL,
    folder_id integer,
    inv_folder_id integer,
    folder_order integer
);


ALTER TABLE public.upload_folders_parent_links OWNER TO postgres;

--
-- TOC entry 259 (class 1259 OID 16600)
-- Name: upload_folders_parent_links_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.upload_folders_parent_links_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.upload_folders_parent_links_id_seq OWNER TO postgres;

--
-- TOC entry 4027 (class 0 OID 0)
-- Dependencies: 259
-- Name: upload_folders_parent_links_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.upload_folders_parent_links_id_seq OWNED BY public.upload_folders_parent_links.id;


--
-- TOC entry 3160 (class 2604 OID 16602)
-- Name: admin_permissions id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.admin_permissions ALTER COLUMN id SET DEFAULT nextval('public.admin_permissions_id_seq'::regclass);


--
-- TOC entry 3161 (class 2604 OID 16603)
-- Name: admin_permissions_role_links id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.admin_permissions_role_links ALTER COLUMN id SET DEFAULT nextval('public.admin_permissions_role_links_id_seq'::regclass);


--
-- TOC entry 3162 (class 2604 OID 16604)
-- Name: admin_roles id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.admin_roles ALTER COLUMN id SET DEFAULT nextval('public.admin_roles_id_seq'::regclass);


--
-- TOC entry 3163 (class 2604 OID 16605)
-- Name: admin_users id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.admin_users ALTER COLUMN id SET DEFAULT nextval('public.admin_users_id_seq'::regclass);


--
-- TOC entry 3164 (class 2604 OID 16606)
-- Name: admin_users_roles_links id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.admin_users_roles_links ALTER COLUMN id SET DEFAULT nextval('public.admin_users_roles_links_id_seq'::regclass);


--
-- TOC entry 3210 (class 2604 OID 214098)
-- Name: available_products_pages id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.available_products_pages ALTER COLUMN id SET DEFAULT nextval('public.available_products_pages_id_seq'::regclass);


--
-- TOC entry 3211 (class 2604 OID 214108)
-- Name: available_products_pages_components id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.available_products_pages_components ALTER COLUMN id SET DEFAULT nextval('public.available_products_pages_components_id_seq'::regclass);


--
-- TOC entry 3212 (class 2604 OID 214124)
-- Name: available_products_pages_localizations_links id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.available_products_pages_localizations_links ALTER COLUMN id SET DEFAULT nextval('public.available_products_pages_localizations_links_id_seq'::regclass);


--
-- TOC entry 3219 (class 2604 OID 214417)
-- Name: basket_pages id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.basket_pages ALTER COLUMN id SET DEFAULT nextval('public.basket_pages_id_seq'::regclass);


--
-- TOC entry 3220 (class 2604 OID 214427)
-- Name: basket_pages_components id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.basket_pages_components ALTER COLUMN id SET DEFAULT nextval('public.basket_pages_components_id_seq'::regclass);


--
-- TOC entry 3221 (class 2604 OID 214443)
-- Name: basket_pages_localizations_links id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.basket_pages_localizations_links ALTER COLUMN id SET DEFAULT nextval('public.basket_pages_localizations_links_id_seq'::regclass);


--
-- TOC entry 3165 (class 2604 OID 16607)
-- Name: components_blocks_hero_slider_items id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.components_blocks_hero_slider_items ALTER COLUMN id SET DEFAULT nextval('public.components_blocks_hero_slider_items_id_seq'::regclass);


--
-- TOC entry 3166 (class 2604 OID 16608)
-- Name: components_blocks_hero_slider_items_components id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.components_blocks_hero_slider_items_components ALTER COLUMN id SET DEFAULT nextval('public.components_blocks_hero_slider_items_components_id_seq'::regclass);


--
-- TOC entry 3167 (class 2604 OID 16609)
-- Name: components_blocks_hero_sliders id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.components_blocks_hero_sliders ALTER COLUMN id SET DEFAULT nextval('public.components_blocks_hero_sliders_id_seq'::regclass);


--
-- TOC entry 3168 (class 2604 OID 16610)
-- Name: components_blocks_hero_sliders_components id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.components_blocks_hero_sliders_components ALTER COLUMN id SET DEFAULT nextval('public.components_blocks_hero_sliders_components_id_seq'::regclass);


--
-- TOC entry 3169 (class 2604 OID 16611)
-- Name: components_shared_links id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.components_shared_links ALTER COLUMN id SET DEFAULT nextval('public.components_shared_links_id_seq'::regclass);


--
-- TOC entry 3170 (class 2604 OID 16612)
-- Name: components_shared_seos id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.components_shared_seos ALTER COLUMN id SET DEFAULT nextval('public.components_shared_seos_id_seq'::regclass);


--
-- TOC entry 3192 (class 2604 OID 213621)
-- Name: dashboard_pages id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.dashboard_pages ALTER COLUMN id SET DEFAULT nextval('public.dashboard_pages_id_seq'::regclass);


--
-- TOC entry 3198 (class 2604 OID 213691)
-- Name: dashboard_pages_components id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.dashboard_pages_components ALTER COLUMN id SET DEFAULT nextval('public.dashboard_pages_components_id_seq'::regclass);


--
-- TOC entry 3199 (class 2604 OID 213707)
-- Name: dashboard_pages_localizations_links id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.dashboard_pages_localizations_links ALTER COLUMN id SET DEFAULT nextval('public.dashboard_pages_localizations_links_id_seq'::regclass);


--
-- TOC entry 3193 (class 2604 OID 213631)
-- Name: download_center_pages id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.download_center_pages ALTER COLUMN id SET DEFAULT nextval('public.download_center_pages_id_seq'::regclass);


--
-- TOC entry 3200 (class 2604 OID 213720)
-- Name: download_center_pages_components id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.download_center_pages_components ALTER COLUMN id SET DEFAULT nextval('public.download_center_pages_components_id_seq'::regclass);


--
-- TOC entry 3201 (class 2604 OID 213736)
-- Name: download_center_pages_localizations_links id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.download_center_pages_localizations_links ALTER COLUMN id SET DEFAULT nextval('public.download_center_pages_localizations_links_id_seq'::regclass);


--
-- TOC entry 3171 (class 2604 OID 16613)
-- Name: files id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.files ALTER COLUMN id SET DEFAULT nextval('public.files_id_seq'::regclass);


--
-- TOC entry 3172 (class 2604 OID 16614)
-- Name: files_folder_links id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.files_folder_links ALTER COLUMN id SET DEFAULT nextval('public.files_folder_links_id_seq'::regclass);


--
-- TOC entry 3173 (class 2604 OID 16615)
-- Name: files_related_morphs id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.files_related_morphs ALTER COLUMN id SET DEFAULT nextval('public.files_related_morphs_id_seq'::regclass);


--
-- TOC entry 3174 (class 2604 OID 16616)
-- Name: home_pages id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.home_pages ALTER COLUMN id SET DEFAULT nextval('public.home_pages_id_seq'::regclass);


--
-- TOC entry 3175 (class 2604 OID 16617)
-- Name: home_pages_components id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.home_pages_components ALTER COLUMN id SET DEFAULT nextval('public.home_pages_components_id_seq'::regclass);


--
-- TOC entry 3176 (class 2604 OID 16618)
-- Name: home_pages_localizations_links id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.home_pages_localizations_links ALTER COLUMN id SET DEFAULT nextval('public.home_pages_localizations_links_id_seq'::regclass);


--
-- TOC entry 3177 (class 2604 OID 16619)
-- Name: i18n_locale id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.i18n_locale ALTER COLUMN id SET DEFAULT nextval('public.i18n_locale_id_seq'::regclass);


--
-- TOC entry 3194 (class 2604 OID 213651)
-- Name: news_pages id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.news_pages ALTER COLUMN id SET DEFAULT nextval('public.news_pages_id_seq'::regclass);


--
-- TOC entry 3202 (class 2604 OID 213778)
-- Name: news_pages_components id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.news_pages_components ALTER COLUMN id SET DEFAULT nextval('public.news_pages_components_id_seq'::regclass);


--
-- TOC entry 3203 (class 2604 OID 213794)
-- Name: news_pages_localizations_links id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.news_pages_localizations_links ALTER COLUMN id SET DEFAULT nextval('public.news_pages_localizations_links_id_seq'::regclass);


--
-- TOC entry 3222 (class 2604 OID 214508)
-- Name: order_item_pages id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_item_pages ALTER COLUMN id SET DEFAULT nextval('public.order_item_pages_id_seq'::regclass);


--
-- TOC entry 3223 (class 2604 OID 214518)
-- Name: order_item_pages_components id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_item_pages_components ALTER COLUMN id SET DEFAULT nextval('public.order_item_pages_components_id_seq'::regclass);


--
-- TOC entry 3224 (class 2604 OID 214534)
-- Name: order_item_pages_localizations_links id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_item_pages_localizations_links ALTER COLUMN id SET DEFAULT nextval('public.order_item_pages_localizations_links_id_seq'::regclass);


--
-- TOC entry 3195 (class 2604 OID 213661)
-- Name: order_pages id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_pages ALTER COLUMN id SET DEFAULT nextval('public.order_pages_id_seq'::regclass);


--
-- TOC entry 3204 (class 2604 OID 213807)
-- Name: order_pages_components id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_pages_components ALTER COLUMN id SET DEFAULT nextval('public.order_pages_components_id_seq'::regclass);


--
-- TOC entry 3205 (class 2604 OID 213823)
-- Name: order_pages_localizations_links id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_pages_localizations_links ALTER COLUMN id SET DEFAULT nextval('public.order_pages_localizations_links_id_seq'::regclass);


--
-- TOC entry 3196 (class 2604 OID 213671)
-- Name: orders_pages id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orders_pages ALTER COLUMN id SET DEFAULT nextval('public.orders_pages_id_seq'::regclass);


--
-- TOC entry 3206 (class 2604 OID 213836)
-- Name: orders_pages_components id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orders_pages_components ALTER COLUMN id SET DEFAULT nextval('public.orders_pages_components_id_seq'::regclass);


--
-- TOC entry 3207 (class 2604 OID 213852)
-- Name: orders_pages_localizations_links id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orders_pages_localizations_links ALTER COLUMN id SET DEFAULT nextval('public.orders_pages_localizations_links_id_seq'::regclass);


--
-- TOC entry 3197 (class 2604 OID 213681)
-- Name: outlet_pages id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.outlet_pages ALTER COLUMN id SET DEFAULT nextval('public.outlet_pages_id_seq'::regclass);


--
-- TOC entry 3208 (class 2604 OID 213865)
-- Name: outlet_pages_components id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.outlet_pages_components ALTER COLUMN id SET DEFAULT nextval('public.outlet_pages_components_id_seq'::regclass);


--
-- TOC entry 3209 (class 2604 OID 213881)
-- Name: outlet_pages_localizations_links id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.outlet_pages_localizations_links ALTER COLUMN id SET DEFAULT nextval('public.outlet_pages_localizations_links_id_seq'::regclass);


--
-- TOC entry 3216 (class 2604 OID 214274)
-- Name: privacy_policy_pages id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.privacy_policy_pages ALTER COLUMN id SET DEFAULT nextval('public.privacy_policy_pages_id_seq'::regclass);


--
-- TOC entry 3217 (class 2604 OID 214287)
-- Name: privacy_policy_pages_components id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.privacy_policy_pages_components ALTER COLUMN id SET DEFAULT nextval('public.privacy_policy_pages_components_id_seq'::regclass);


--
-- TOC entry 3218 (class 2604 OID 214303)
-- Name: privacy_policy_pages_localizations_links id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.privacy_policy_pages_localizations_links ALTER COLUMN id SET DEFAULT nextval('public.privacy_policy_pages_localizations_links_id_seq'::regclass);


--
-- TOC entry 3213 (class 2604 OID 214184)
-- Name: regulations_pages id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.regulations_pages ALTER COLUMN id SET DEFAULT nextval('public.regulations_pages_id_seq'::regclass);


--
-- TOC entry 3214 (class 2604 OID 214197)
-- Name: regulations_pages_components id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.regulations_pages_components ALTER COLUMN id SET DEFAULT nextval('public.regulations_pages_components_id_seq'::regclass);


--
-- TOC entry 3215 (class 2604 OID 214213)
-- Name: regulations_pages_localizations_links id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.regulations_pages_localizations_links ALTER COLUMN id SET DEFAULT nextval('public.regulations_pages_localizations_links_id_seq'::regclass);


--
-- TOC entry 3225 (class 2604 OID 214600)
-- Name: search_products_pages id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.search_products_pages ALTER COLUMN id SET DEFAULT nextval('public.search_products_pages_id_seq'::regclass);


--
-- TOC entry 3226 (class 2604 OID 214610)
-- Name: search_products_pages_components id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.search_products_pages_components ALTER COLUMN id SET DEFAULT nextval('public.search_products_pages_components_id_seq'::regclass);


--
-- TOC entry 3227 (class 2604 OID 214626)
-- Name: search_products_pages_localizations_links id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.search_products_pages_localizations_links ALTER COLUMN id SET DEFAULT nextval('public.search_products_pages_localizations_links_id_seq'::regclass);


--
-- TOC entry 3178 (class 2604 OID 16620)
-- Name: strapi_api_token_permissions id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.strapi_api_token_permissions ALTER COLUMN id SET DEFAULT nextval('public.strapi_api_token_permissions_id_seq'::regclass);


--
-- TOC entry 3179 (class 2604 OID 16621)
-- Name: strapi_api_token_permissions_token_links id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.strapi_api_token_permissions_token_links ALTER COLUMN id SET DEFAULT nextval('public.strapi_api_token_permissions_token_links_id_seq'::regclass);


--
-- TOC entry 3180 (class 2604 OID 16622)
-- Name: strapi_api_tokens id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.strapi_api_tokens ALTER COLUMN id SET DEFAULT nextval('public.strapi_api_tokens_id_seq'::regclass);


--
-- TOC entry 3181 (class 2604 OID 16623)
-- Name: strapi_core_store_settings id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.strapi_core_store_settings ALTER COLUMN id SET DEFAULT nextval('public.strapi_core_store_settings_id_seq'::regclass);


--
-- TOC entry 3182 (class 2604 OID 16624)
-- Name: strapi_database_schema id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.strapi_database_schema ALTER COLUMN id SET DEFAULT nextval('public.strapi_database_schema_id_seq'::regclass);


--
-- TOC entry 3183 (class 2604 OID 16625)
-- Name: strapi_migrations id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.strapi_migrations ALTER COLUMN id SET DEFAULT nextval('public.strapi_migrations_id_seq'::regclass);


--
-- TOC entry 3184 (class 2604 OID 16626)
-- Name: strapi_webhooks id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.strapi_webhooks ALTER COLUMN id SET DEFAULT nextval('public.strapi_webhooks_id_seq'::regclass);


--
-- TOC entry 3185 (class 2604 OID 16627)
-- Name: up_permissions id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.up_permissions ALTER COLUMN id SET DEFAULT nextval('public.up_permissions_id_seq'::regclass);


--
-- TOC entry 3186 (class 2604 OID 16628)
-- Name: up_permissions_role_links id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.up_permissions_role_links ALTER COLUMN id SET DEFAULT nextval('public.up_permissions_role_links_id_seq'::regclass);


--
-- TOC entry 3187 (class 2604 OID 16629)
-- Name: up_roles id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.up_roles ALTER COLUMN id SET DEFAULT nextval('public.up_roles_id_seq'::regclass);


--
-- TOC entry 3188 (class 2604 OID 16630)
-- Name: up_users id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.up_users ALTER COLUMN id SET DEFAULT nextval('public.up_users_id_seq'::regclass);


--
-- TOC entry 3189 (class 2604 OID 16631)
-- Name: up_users_role_links id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.up_users_role_links ALTER COLUMN id SET DEFAULT nextval('public.up_users_role_links_id_seq'::regclass);


--
-- TOC entry 3190 (class 2604 OID 16632)
-- Name: upload_folders id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.upload_folders ALTER COLUMN id SET DEFAULT nextval('public.upload_folders_id_seq'::regclass);


--
-- TOC entry 3191 (class 2604 OID 16633)
-- Name: upload_folders_parent_links id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.upload_folders_parent_links ALTER COLUMN id SET DEFAULT nextval('public.upload_folders_parent_links_id_seq'::regclass);


--
-- TOC entry 3819 (class 0 OID 16384)
-- Dependencies: 196
-- Data for Name: admin_permissions; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (1, 'plugin::upload.read', NULL, '{}', '[]', '2022-12-03 18:53:08.049', '2022-12-03 18:53:08.049', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (2, 'plugin::upload.assets.create', NULL, '{}', '[]', '2022-12-03 18:53:08.081', '2022-12-03 18:53:08.081', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (3, 'plugin::upload.assets.update', NULL, '{}', '[]', '2022-12-03 18:53:08.116', '2022-12-03 18:53:08.116', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (4, 'plugin::upload.assets.download', NULL, '{}', '[]', '2022-12-03 18:53:08.142', '2022-12-03 18:53:08.142', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (5, 'plugin::upload.assets.copy-link', NULL, '{}', '[]', '2022-12-03 18:53:08.165', '2022-12-03 18:53:08.165', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (6, 'plugin::upload.read', NULL, '{}', '["admin::is-creator"]', '2022-12-03 18:53:08.194', '2022-12-03 18:53:08.194', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (7, 'plugin::upload.assets.create', NULL, '{}', '[]', '2022-12-03 18:53:08.228', '2022-12-03 18:53:08.228', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (8, 'plugin::upload.assets.update', NULL, '{}', '["admin::is-creator"]', '2022-12-03 18:53:08.266', '2022-12-03 18:53:08.266', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (9, 'plugin::upload.assets.download', NULL, '{}', '[]', '2022-12-03 18:53:08.301', '2022-12-03 18:53:08.301', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (10, 'plugin::upload.assets.copy-link', NULL, '{}', '[]', '2022-12-03 18:53:08.344', '2022-12-03 18:53:08.344', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (11, 'plugin::content-manager.explorer.create', 'plugin::users-permissions.user', '{"fields": ["username", "email", "provider", "password", "resetPasswordToken", "confirmationToken", "confirmed", "blocked", "role"]}', '[]', '2022-12-03 18:53:08.447', '2022-12-03 18:53:08.447', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (12, 'plugin::content-manager.explorer.read', 'plugin::users-permissions.user', '{"fields": ["username", "email", "provider", "password", "resetPasswordToken", "confirmationToken", "confirmed", "blocked", "role"]}', '[]', '2022-12-03 18:53:08.479', '2022-12-03 18:53:08.479', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (13, 'plugin::content-manager.explorer.update', 'plugin::users-permissions.user', '{"fields": ["username", "email", "provider", "password", "resetPasswordToken", "confirmationToken", "confirmed", "blocked", "role"]}', '[]', '2022-12-03 18:53:08.51', '2022-12-03 18:53:08.51', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (14, 'plugin::content-manager.explorer.delete', 'plugin::users-permissions.user', '{}', '[]', '2022-12-03 18:53:08.533', '2022-12-03 18:53:08.533', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (15, 'plugin::content-manager.single-types.configure-view', NULL, '{}', '[]', '2022-12-03 18:53:08.559', '2022-12-03 18:53:08.559', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (16, 'plugin::content-manager.collection-types.configure-view', NULL, '{}', '[]', '2022-12-03 18:53:08.581', '2022-12-03 18:53:08.581', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (17, 'plugin::content-manager.components.configure-layout', NULL, '{}', '[]', '2022-12-03 18:53:08.603', '2022-12-03 18:53:08.603', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (18, 'plugin::content-type-builder.read', NULL, '{}', '[]', '2022-12-03 18:53:08.633', '2022-12-03 18:53:08.633', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (19, 'plugin::email.settings.read', NULL, '{}', '[]', '2022-12-03 18:53:08.66', '2022-12-03 18:53:08.66', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (20, 'plugin::upload.read', NULL, '{}', '[]', '2022-12-03 18:53:08.682', '2022-12-03 18:53:08.682', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (21, 'plugin::upload.assets.create', NULL, '{}', '[]', '2022-12-03 18:53:08.713', '2022-12-03 18:53:08.713', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (22, 'plugin::upload.assets.update', NULL, '{}', '[]', '2022-12-03 18:53:08.758', '2022-12-03 18:53:08.758', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (23, 'plugin::upload.assets.download', NULL, '{}', '[]', '2022-12-03 18:53:08.789', '2022-12-03 18:53:08.789', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (24, 'plugin::upload.assets.copy-link', NULL, '{}', '[]', '2022-12-03 18:53:08.823', '2022-12-03 18:53:08.823', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (25, 'plugin::upload.settings.read', NULL, '{}', '[]', '2022-12-03 18:53:08.85', '2022-12-03 18:53:08.85', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (26, 'plugin::users-permissions.roles.create', NULL, '{}', '[]', '2022-12-03 18:53:08.879', '2022-12-03 18:53:08.879', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (27, 'plugin::users-permissions.roles.read', NULL, '{}', '[]', '2022-12-03 18:53:08.909', '2022-12-03 18:53:08.909', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (28, 'plugin::users-permissions.roles.update', NULL, '{}', '[]', '2022-12-03 18:53:08.939', '2022-12-03 18:53:08.939', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (29, 'plugin::users-permissions.roles.delete', NULL, '{}', '[]', '2022-12-03 18:53:08.973', '2022-12-03 18:53:08.973', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (30, 'plugin::users-permissions.providers.read', NULL, '{}', '[]', '2022-12-03 18:53:09.008', '2022-12-03 18:53:09.008', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (31, 'plugin::users-permissions.providers.update', NULL, '{}', '[]', '2022-12-03 18:53:09.038', '2022-12-03 18:53:09.038', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (32, 'plugin::users-permissions.email-templates.read', NULL, '{}', '[]', '2022-12-03 18:53:09.062', '2022-12-03 18:53:09.062', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (33, 'plugin::users-permissions.email-templates.update', NULL, '{}', '[]', '2022-12-03 18:53:09.094', '2022-12-03 18:53:09.094', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (34, 'plugin::users-permissions.advanced-settings.read', NULL, '{}', '[]', '2022-12-03 18:53:09.118', '2022-12-03 18:53:09.118', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (35, 'plugin::users-permissions.advanced-settings.update', NULL, '{}', '[]', '2022-12-03 18:53:09.145', '2022-12-03 18:53:09.145', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (36, 'plugin::i18n.locale.create', NULL, '{}', '[]', '2022-12-03 18:53:09.168', '2022-12-03 18:53:09.168', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (37, 'plugin::i18n.locale.read', NULL, '{}', '[]', '2022-12-03 18:53:09.201', '2022-12-03 18:53:09.201', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (38, 'plugin::i18n.locale.update', NULL, '{}', '[]', '2022-12-03 18:53:09.235', '2022-12-03 18:53:09.235', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (39, 'plugin::i18n.locale.delete', NULL, '{}', '[]', '2022-12-03 18:53:09.269', '2022-12-03 18:53:09.269', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (40, 'admin::marketplace.read', NULL, '{}', '[]', '2022-12-03 18:53:09.304', '2022-12-03 18:53:09.304', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (41, 'admin::marketplace.plugins.install', NULL, '{}', '[]', '2022-12-03 18:53:09.335', '2022-12-03 18:53:09.335', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (42, 'admin::marketplace.plugins.uninstall', NULL, '{}', '[]', '2022-12-03 18:53:09.369', '2022-12-03 18:53:09.369', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (43, 'admin::webhooks.create', NULL, '{}', '[]', '2022-12-03 18:53:09.408', '2022-12-03 18:53:09.408', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (44, 'admin::webhooks.read', NULL, '{}', '[]', '2022-12-03 18:53:09.447', '2022-12-03 18:53:09.447', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (45, 'admin::webhooks.update', NULL, '{}', '[]', '2022-12-03 18:53:09.481', '2022-12-03 18:53:09.481', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (46, 'admin::webhooks.delete', NULL, '{}', '[]', '2022-12-03 18:53:09.512', '2022-12-03 18:53:09.512', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (47, 'admin::users.create', NULL, '{}', '[]', '2022-12-03 18:53:09.55', '2022-12-03 18:53:09.55', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (48, 'admin::users.read', NULL, '{}', '[]', '2022-12-03 18:53:09.587', '2022-12-03 18:53:09.587', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (49, 'admin::users.update', NULL, '{}', '[]', '2022-12-03 18:53:09.619', '2022-12-03 18:53:09.619', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (50, 'admin::users.delete', NULL, '{}', '[]', '2022-12-03 18:53:09.645', '2022-12-03 18:53:09.645', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (51, 'admin::roles.create', NULL, '{}', '[]', '2022-12-03 18:53:09.685', '2022-12-03 18:53:09.685', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (52, 'admin::roles.read', NULL, '{}', '[]', '2022-12-03 18:53:09.718', '2022-12-03 18:53:09.718', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (53, 'admin::roles.update', NULL, '{}', '[]', '2022-12-03 18:53:09.753', '2022-12-03 18:53:09.753', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (54, 'admin::roles.delete', NULL, '{}', '[]', '2022-12-03 18:53:09.782', '2022-12-03 18:53:09.782', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (55, 'admin::api-tokens.access', NULL, '{}', '[]', '2022-12-03 18:53:09.812', '2022-12-03 18:53:09.812', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (56, 'admin::api-tokens.create', NULL, '{}', '[]', '2022-12-03 18:53:09.856', '2022-12-03 18:53:09.856', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (57, 'admin::api-tokens.read', NULL, '{}', '[]', '2022-12-03 18:53:09.892', '2022-12-03 18:53:09.892', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (58, 'admin::api-tokens.update', NULL, '{}', '[]', '2022-12-03 18:53:09.925', '2022-12-03 18:53:09.925', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (59, 'admin::api-tokens.regenerate', NULL, '{}', '[]', '2022-12-03 18:53:09.962', '2022-12-03 18:53:09.962', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (60, 'admin::api-tokens.delete', NULL, '{}', '[]', '2022-12-03 18:53:09.995', '2022-12-03 18:53:09.995', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (61, 'admin::project-settings.update', NULL, '{}', '[]', '2022-12-03 18:53:10.036', '2022-12-03 18:53:10.036', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (62, 'admin::project-settings.read', NULL, '{}', '[]', '2022-12-03 18:53:10.067', '2022-12-03 18:53:10.067', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (65, 'plugin::content-manager.explorer.update', 'api::home-page.home-page', '{"fields": ["seo.metaTitle", "seo.metaDescription", "heroSlider.heroSliderItems.image", "heroSlider.heroSliderItems.title", "heroSlider.heroSliderItems.text", "heroSlider.heroSliderItems.cta.href", "heroSlider.heroSliderItems.cta.label", "heroSlider.heroSliderItems.cta.target", "heroSlider.heroSliderItems.cta.isExternal"], "locales": ["en", "pl", "de"]}', '[]', '2022-12-03 21:47:40.5', '2022-12-03 21:47:40.5', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (63, 'plugin::content-manager.explorer.create', 'api::home-page.home-page', '{"fields": ["seo.metaTitle", "seo.metaDescription", "heroSlider.heroSliderItems.image", "heroSlider.heroSliderItems.title", "heroSlider.heroSliderItems.text", "heroSlider.heroSliderItems.cta.href", "heroSlider.heroSliderItems.cta.label", "heroSlider.heroSliderItems.cta.target", "heroSlider.heroSliderItems.cta.isExternal"], "locales": ["en", "pl", "de"]}', '[]', '2022-12-03 21:47:40.513', '2022-12-03 21:47:40.513', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (66, 'plugin::content-manager.explorer.delete', 'api::home-page.home-page', '{"locales": ["en", "pl", "de"]}', '[]', '2022-12-03 21:47:40.525', '2022-12-03 21:47:40.525', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (67, 'plugin::content-manager.explorer.publish', 'api::home-page.home-page', '{"locales": ["en", "pl", "de"]}', '[]', '2022-12-03 21:47:40.536', '2022-12-03 21:47:40.536', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (64, 'plugin::content-manager.explorer.read', 'api::home-page.home-page', '{"fields": ["seo.metaTitle", "seo.metaDescription", "heroSlider.heroSliderItems.image", "heroSlider.heroSliderItems.title", "heroSlider.heroSliderItems.text", "heroSlider.heroSliderItems.cta.href", "heroSlider.heroSliderItems.cta.label", "heroSlider.heroSliderItems.cta.target", "heroSlider.heroSliderItems.cta.isExternal"], "locales": ["en", "pl", "de"]}', '[]', '2022-12-03 21:47:40.55', '2022-12-03 21:47:40.55', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (257, 'plugin::content-manager.explorer.create', 'api::basket-page.basket-page', '{"fields": ["seo.metaTitle", "seo.metaDescription"], "locales": ["en", "pl", "de"]}', '[]', '2022-12-14 12:02:29.439', '2022-12-14 12:02:29.439', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (258, 'plugin::content-manager.explorer.read', 'api::basket-page.basket-page', '{"fields": ["seo.metaTitle", "seo.metaDescription"], "locales": ["en", "pl", "de"]}', '[]', '2022-12-14 12:02:29.458', '2022-12-14 12:02:29.458', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (259, 'plugin::content-manager.explorer.update', 'api::basket-page.basket-page', '{"fields": ["seo.metaTitle", "seo.metaDescription"], "locales": ["en", "pl", "de"]}', '[]', '2022-12-14 12:02:29.476', '2022-12-14 12:02:29.476', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (260, 'plugin::content-manager.explorer.delete', 'api::basket-page.basket-page', '{"locales": ["en", "pl", "de"]}', '[]', '2022-12-14 12:02:29.493', '2022-12-14 12:02:29.493', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (261, 'plugin::content-manager.explorer.publish', 'api::basket-page.basket-page', '{"locales": ["en", "pl", "de"]}', '[]', '2022-12-14 12:02:29.514', '2022-12-14 12:02:29.514', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (262, 'plugin::content-manager.explorer.create', 'api::order-item-page.order-item-page', '{"fields": ["seo.metaTitle", "seo.metaDescription"], "locales": ["en", "pl", "de"]}', '[]', '2022-12-14 12:03:24.365', '2022-12-14 12:03:24.365', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (263, 'plugin::content-manager.explorer.read', 'api::order-item-page.order-item-page', '{"fields": ["seo.metaTitle", "seo.metaDescription"], "locales": ["en", "pl", "de"]}', '[]', '2022-12-14 12:03:24.385', '2022-12-14 12:03:24.385', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (264, 'plugin::content-manager.explorer.update', 'api::order-item-page.order-item-page', '{"fields": ["seo.metaTitle", "seo.metaDescription"], "locales": ["en", "pl", "de"]}', '[]', '2022-12-14 12:03:24.404', '2022-12-14 12:03:24.404', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (265, 'plugin::content-manager.explorer.delete', 'api::order-item-page.order-item-page', '{"locales": ["en", "pl", "de"]}', '[]', '2022-12-14 12:03:24.419', '2022-12-14 12:03:24.419', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (266, 'plugin::content-manager.explorer.publish', 'api::order-item-page.order-item-page', '{"locales": ["en", "pl", "de"]}', '[]', '2022-12-14 12:03:24.434', '2022-12-14 12:03:24.434', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (267, 'plugin::content-manager.explorer.create', 'api::search-products-page.search-products-page', '{"fields": ["seo.metaTitle", "seo.metaDescription"], "locales": ["en", "pl", "de"]}', '[]', '2022-12-14 12:08:57.459', '2022-12-14 12:08:57.459', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (268, 'plugin::content-manager.explorer.read', 'api::search-products-page.search-products-page', '{"fields": ["seo.metaTitle", "seo.metaDescription"], "locales": ["en", "pl", "de"]}', '[]', '2022-12-14 12:08:57.493', '2022-12-14 12:08:57.493', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (269, 'plugin::content-manager.explorer.update', 'api::search-products-page.search-products-page', '{"fields": ["seo.metaTitle", "seo.metaDescription"], "locales": ["en", "pl", "de"]}', '[]', '2022-12-14 12:08:57.519', '2022-12-14 12:08:57.519', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (270, 'plugin::content-manager.explorer.delete', 'api::search-products-page.search-products-page', '{"locales": ["en", "pl", "de"]}', '[]', '2022-12-14 12:08:57.544', '2022-12-14 12:08:57.544', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (271, 'plugin::content-manager.explorer.publish', 'api::search-products-page.search-products-page', '{"locales": ["en", "pl", "de"]}', '[]', '2022-12-14 12:08:57.56', '2022-12-14 12:08:57.56', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (244, 'plugin::content-manager.explorer.delete', 'api::regulations-page.regulations-page', '{"locales": ["en", "pl", "de"]}', '[]', '2022-12-14 11:54:44.861', '2022-12-14 11:54:44.861', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (245, 'plugin::content-manager.explorer.publish', 'api::regulations-page.regulations-page', '{"locales": ["en", "pl", "de"]}', '[]', '2022-12-14 11:54:44.883', '2022-12-14 11:54:44.883', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (249, 'plugin::content-manager.explorer.delete', 'api::privacy-policy-page.privacy-policy-page', '{"locales": ["en", "pl", "de"]}', '[]', '2022-12-14 11:57:51.87', '2022-12-14 11:57:51.87', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (250, 'plugin::content-manager.explorer.publish', 'api::privacy-policy-page.privacy-policy-page', '{"locales": ["en", "pl", "de"]}', '[]', '2022-12-14 11:57:51.905', '2022-12-14 11:57:51.905', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (252, 'plugin::content-manager.explorer.read', 'api::regulations-page.regulations-page', '{"fields": ["seo.metaTitle", "seo.metaDescription", "Text"], "locales": ["en", "pl", "de"]}', '[]', '2022-12-14 11:59:30.953', '2022-12-14 11:59:30.953', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (253, 'plugin::content-manager.explorer.update', 'api::regulations-page.regulations-page', '{"fields": ["seo.metaTitle", "seo.metaDescription", "Text"], "locales": ["en", "pl", "de"]}', '[]', '2022-12-14 11:59:30.972', '2022-12-14 11:59:30.972', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (254, 'plugin::content-manager.explorer.create', 'api::privacy-policy-page.privacy-policy-page', '{"fields": ["seo.metaTitle", "seo.metaDescription", "Text"], "locales": ["en", "pl", "de"]}', '[]', '2022-12-14 11:59:48.594', '2022-12-14 11:59:48.594', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (255, 'plugin::content-manager.explorer.read', 'api::privacy-policy-page.privacy-policy-page', '{"fields": ["seo.metaTitle", "seo.metaDescription", "Text"], "locales": ["en", "pl", "de"]}', '[]', '2022-12-14 11:59:48.611', '2022-12-14 11:59:48.611', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (256, 'plugin::content-manager.explorer.update', 'api::privacy-policy-page.privacy-policy-page', '{"fields": ["seo.metaTitle", "seo.metaDescription", "Text"], "locales": ["en", "pl", "de"]}', '[]', '2022-12-14 11:59:48.633', '2022-12-14 11:59:48.633', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (201, 'plugin::content-manager.explorer.create', 'api::dashboard-page.dashboard-page', '{"fields": ["seo.metaTitle", "seo.metaDescription"], "locales": ["en", "pl", "de"]}', '[]', '2022-12-14 09:19:13.071', '2022-12-14 09:19:13.071', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (202, 'plugin::content-manager.explorer.create', 'api::download-center-page.download-center-page', '{"fields": ["seo.metaTitle", "seo.metaDescription"], "locales": ["en", "pl", "de"]}', '[]', '2022-12-14 09:19:13.097', '2022-12-14 09:19:13.097', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (204, 'plugin::content-manager.explorer.create', 'api::news-page.news-page', '{"fields": ["seo.metaTitle", "seo.metaDescription"], "locales": ["en", "pl", "de"]}', '[]', '2022-12-14 09:19:13.127', '2022-12-14 09:19:13.127', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (205, 'plugin::content-manager.explorer.create', 'api::order-page.order-page', '{"fields": ["seo.metaTitle", "seo.metaDescription"], "locales": ["en", "pl", "de"]}', '[]', '2022-12-14 09:19:13.154', '2022-12-14 09:19:13.154', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (206, 'plugin::content-manager.explorer.create', 'api::orders-page.orders-page', '{"fields": ["seo.metaTitle", "seo.metaDescription"], "locales": ["en", "pl", "de"]}', '[]', '2022-12-14 09:19:13.168', '2022-12-14 09:19:13.168', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (207, 'plugin::content-manager.explorer.create', 'api::outlet-page.outlet-page', '{"fields": ["seo.metaTitle", "seo.metaDescription"], "locales": ["en", "pl", "de"]}', '[]', '2022-12-14 09:19:13.184', '2022-12-14 09:19:13.184', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (208, 'plugin::content-manager.explorer.read', 'api::dashboard-page.dashboard-page', '{"fields": ["seo.metaTitle", "seo.metaDescription"], "locales": ["en", "pl", "de"]}', '[]', '2022-12-14 09:19:13.2', '2022-12-14 09:19:13.2', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (209, 'plugin::content-manager.explorer.read', 'api::download-center-page.download-center-page', '{"fields": ["seo.metaTitle", "seo.metaDescription"], "locales": ["en", "pl", "de"]}', '[]', '2022-12-14 09:19:13.213', '2022-12-14 09:19:13.213', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (211, 'plugin::content-manager.explorer.read', 'api::news-page.news-page', '{"fields": ["seo.metaTitle", "seo.metaDescription"], "locales": ["en", "pl", "de"]}', '[]', '2022-12-14 09:19:13.243', '2022-12-14 09:19:13.243', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (212, 'plugin::content-manager.explorer.read', 'api::order-page.order-page', '{"fields": ["seo.metaTitle", "seo.metaDescription"], "locales": ["en", "pl", "de"]}', '[]', '2022-12-14 09:19:13.256', '2022-12-14 09:19:13.256', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (213, 'plugin::content-manager.explorer.read', 'api::orders-page.orders-page', '{"fields": ["seo.metaTitle", "seo.metaDescription"], "locales": ["en", "pl", "de"]}', '[]', '2022-12-14 09:19:13.272', '2022-12-14 09:19:13.272', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (214, 'plugin::content-manager.explorer.read', 'api::outlet-page.outlet-page', '{"fields": ["seo.metaTitle", "seo.metaDescription"], "locales": ["en", "pl", "de"]}', '[]', '2022-12-14 09:19:13.286', '2022-12-14 09:19:13.286', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (215, 'plugin::content-manager.explorer.update', 'api::dashboard-page.dashboard-page', '{"fields": ["seo.metaTitle", "seo.metaDescription"], "locales": ["en", "pl", "de"]}', '[]', '2022-12-14 09:19:13.301', '2022-12-14 09:19:13.301', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (216, 'plugin::content-manager.explorer.update', 'api::download-center-page.download-center-page', '{"fields": ["seo.metaTitle", "seo.metaDescription"], "locales": ["en", "pl", "de"]}', '[]', '2022-12-14 09:19:13.316', '2022-12-14 09:19:13.316', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (218, 'plugin::content-manager.explorer.update', 'api::news-page.news-page', '{"fields": ["seo.metaTitle", "seo.metaDescription"], "locales": ["en", "pl", "de"]}', '[]', '2022-12-14 09:19:13.357', '2022-12-14 09:19:13.357', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (219, 'plugin::content-manager.explorer.update', 'api::order-page.order-page', '{"fields": ["seo.metaTitle", "seo.metaDescription"], "locales": ["en", "pl", "de"]}', '[]', '2022-12-14 09:19:13.374', '2022-12-14 09:19:13.374', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (220, 'plugin::content-manager.explorer.update', 'api::orders-page.orders-page', '{"fields": ["seo.metaTitle", "seo.metaDescription"], "locales": ["en", "pl", "de"]}', '[]', '2022-12-14 09:19:13.394', '2022-12-14 09:19:13.394', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (221, 'plugin::content-manager.explorer.update', 'api::outlet-page.outlet-page', '{"fields": ["seo.metaTitle", "seo.metaDescription"], "locales": ["en", "pl", "de"]}', '[]', '2022-12-14 09:19:13.41', '2022-12-14 09:19:13.41', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (222, 'plugin::content-manager.explorer.delete', 'api::dashboard-page.dashboard-page', '{"locales": ["en", "pl", "de"]}', '[]', '2022-12-14 09:19:13.426', '2022-12-14 09:19:13.426', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (223, 'plugin::content-manager.explorer.delete', 'api::download-center-page.download-center-page', '{"locales": ["en", "pl", "de"]}', '[]', '2022-12-14 09:19:13.441', '2022-12-14 09:19:13.441', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (225, 'plugin::content-manager.explorer.delete', 'api::news-page.news-page', '{"locales": ["en", "pl", "de"]}', '[]', '2022-12-14 09:19:13.469', '2022-12-14 09:19:13.469', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (226, 'plugin::content-manager.explorer.delete', 'api::order-page.order-page', '{"locales": ["en", "pl", "de"]}', '[]', '2022-12-14 09:19:13.494', '2022-12-14 09:19:13.494', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (227, 'plugin::content-manager.explorer.delete', 'api::orders-page.orders-page', '{"locales": ["en", "pl", "de"]}', '[]', '2022-12-14 09:19:13.515', '2022-12-14 09:19:13.515', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (228, 'plugin::content-manager.explorer.delete', 'api::outlet-page.outlet-page', '{"locales": ["en", "pl", "de"]}', '[]', '2022-12-14 09:19:13.535', '2022-12-14 09:19:13.535', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (229, 'plugin::content-manager.explorer.publish', 'api::dashboard-page.dashboard-page', '{"locales": ["en", "pl", "de"]}', '[]', '2022-12-14 09:19:13.57', '2022-12-14 09:19:13.57', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (230, 'plugin::content-manager.explorer.publish', 'api::download-center-page.download-center-page', '{"locales": ["en", "pl", "de"]}', '[]', '2022-12-14 09:19:13.586', '2022-12-14 09:19:13.586', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (232, 'plugin::content-manager.explorer.publish', 'api::news-page.news-page', '{"locales": ["en", "pl", "de"]}', '[]', '2022-12-14 09:19:13.621', '2022-12-14 09:19:13.621', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (233, 'plugin::content-manager.explorer.publish', 'api::order-page.order-page', '{"locales": ["en", "pl", "de"]}', '[]', '2022-12-14 09:19:13.638', '2022-12-14 09:19:13.638', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (234, 'plugin::content-manager.explorer.publish', 'api::orders-page.orders-page', '{"locales": ["en", "pl", "de"]}', '[]', '2022-12-14 09:19:13.657', '2022-12-14 09:19:13.657', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (235, 'plugin::content-manager.explorer.publish', 'api::outlet-page.outlet-page', '{"locales": ["en", "pl", "de"]}', '[]', '2022-12-14 09:19:13.672', '2022-12-14 09:19:13.672', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (236, 'plugin::content-manager.explorer.create', 'api::available-products-page.available-products-page', '{"fields": ["seo.metaTitle", "seo.metaDescription"], "locales": ["en", "pl", "de"]}', '[]', '2022-12-14 10:11:40.779', '2022-12-14 10:11:40.779', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (237, 'plugin::content-manager.explorer.read', 'api::available-products-page.available-products-page', '{"fields": ["seo.metaTitle", "seo.metaDescription"], "locales": ["en", "pl", "de"]}', '[]', '2022-12-14 10:11:40.797', '2022-12-14 10:11:40.797', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (238, 'plugin::content-manager.explorer.update', 'api::available-products-page.available-products-page', '{"fields": ["seo.metaTitle", "seo.metaDescription"], "locales": ["en", "pl", "de"]}', '[]', '2022-12-14 10:11:40.813', '2022-12-14 10:11:40.813', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (239, 'plugin::content-manager.explorer.delete', 'api::available-products-page.available-products-page', '{"locales": ["en", "pl", "de"]}', '[]', '2022-12-14 10:11:40.826', '2022-12-14 10:11:40.826', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (240, 'plugin::content-manager.explorer.publish', 'api::available-products-page.available-products-page', '{"locales": ["en", "pl", "de"]}', '[]', '2022-12-14 10:11:40.846', '2022-12-14 10:11:40.846', NULL, NULL);
INSERT INTO public.admin_permissions (id, action, subject, properties, conditions, created_at, updated_at, created_by_id, updated_by_id) VALUES (251, 'plugin::content-manager.explorer.create', 'api::regulations-page.regulations-page', '{"fields": ["seo.metaTitle", "seo.metaDescription", "Text"], "locales": ["en", "pl", "de"]}', '[]', '2022-12-14 11:59:30.936', '2022-12-14 11:59:30.936', NULL, NULL);


--
-- TOC entry 3821 (class 0 OID 16392)
-- Dependencies: 198
-- Data for Name: admin_permissions_role_links; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (1, 1, 2, 1);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (2, 2, 2, 2);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (3, 3, 2, 3);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (4, 4, 2, 4);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (5, 5, 2, 5);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (6, 6, 3, 1);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (7, 7, 3, 2);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (8, 8, 3, 3);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (9, 9, 3, 4);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (10, 10, 3, 5);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (11, 11, 1, 1);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (12, 12, 1, 2);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (13, 13, 1, 3);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (14, 14, 1, 4);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (15, 15, 1, 5);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (16, 16, 1, 6);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (17, 17, 1, 7);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (18, 18, 1, 8);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (19, 19, 1, 9);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (20, 20, 1, 10);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (21, 21, 1, 11);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (22, 22, 1, 12);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (23, 23, 1, 13);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (24, 24, 1, 14);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (25, 25, 1, 15);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (26, 26, 1, 16);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (27, 27, 1, 17);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (28, 28, 1, 18);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (29, 29, 1, 19);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (30, 30, 1, 20);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (31, 31, 1, 21);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (32, 32, 1, 22);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (33, 33, 1, 23);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (34, 34, 1, 24);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (35, 35, 1, 25);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (36, 36, 1, 26);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (37, 37, 1, 27);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (38, 38, 1, 28);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (39, 39, 1, 29);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (40, 40, 1, 30);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (41, 41, 1, 31);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (42, 42, 1, 32);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (43, 43, 1, 33);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (44, 44, 1, 34);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (45, 45, 1, 35);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (46, 46, 1, 36);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (47, 47, 1, 37);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (48, 48, 1, 38);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (49, 49, 1, 39);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (50, 50, 1, 40);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (51, 51, 1, 41);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (52, 52, 1, 42);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (53, 53, 1, 43);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (54, 54, 1, 44);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (55, 55, 1, 45);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (56, 56, 1, 46);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (57, 57, 1, 47);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (58, 58, 1, 48);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (59, 59, 1, 49);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (60, 60, 1, 50);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (61, 61, 1, 51);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (62, 62, 1, 52);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (73, 65, 1, 53);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (74, 63, 1, 54);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (75, 66, 1, 55);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (76, 67, 1, 56);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (77, 64, 1, 57);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (267, 257, 1, 114);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (268, 258, 1, 115);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (269, 259, 1, 116);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (270, 260, 1, 117);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (271, 261, 1, 118);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (272, 262, 1, 119);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (273, 263, 1, 120);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (274, 264, 1, 121);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (275, 265, 1, 122);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (276, 266, 1, 123);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (277, 267, 1, 124);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (278, 268, 1, 125);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (279, 269, 1, 126);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (280, 270, 1, 127);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (281, 271, 1, 128);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (211, 201, 1, 58);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (212, 202, 1, 59);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (214, 204, 1, 61);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (215, 205, 1, 62);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (216, 206, 1, 63);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (217, 207, 1, 64);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (218, 208, 1, 65);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (219, 209, 1, 66);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (221, 211, 1, 68);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (222, 212, 1, 69);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (223, 213, 1, 70);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (224, 214, 1, 71);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (225, 215, 1, 72);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (226, 216, 1, 73);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (228, 218, 1, 75);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (229, 219, 1, 76);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (230, 220, 1, 77);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (231, 221, 1, 78);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (232, 222, 1, 79);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (233, 223, 1, 80);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (235, 225, 1, 82);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (236, 226, 1, 83);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (237, 227, 1, 84);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (238, 228, 1, 85);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (239, 229, 1, 86);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (240, 230, 1, 87);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (242, 232, 1, 89);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (243, 233, 1, 90);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (244, 234, 1, 91);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (245, 235, 1, 92);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (246, 236, 1, 93);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (247, 237, 1, 94);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (248, 238, 1, 95);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (249, 239, 1, 96);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (250, 240, 1, 97);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (254, 244, 1, 101);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (255, 245, 1, 102);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (259, 249, 1, 106);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (260, 250, 1, 107);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (261, 251, 1, 108);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (262, 252, 1, 109);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (263, 253, 1, 110);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (264, 254, 1, 111);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (265, 255, 1, 112);
INSERT INTO public.admin_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (266, 256, 1, 113);


--
-- TOC entry 3823 (class 0 OID 16397)
-- Dependencies: 200
-- Data for Name: admin_roles; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.admin_roles (id, name, code, description, created_at, updated_at, created_by_id, updated_by_id) VALUES (1, 'Super Admin', 'strapi-super-admin', 'Super Admins can access and manage all features and settings.', '2022-12-03 18:53:07.975', '2022-12-03 18:53:07.975', NULL, NULL);
INSERT INTO public.admin_roles (id, name, code, description, created_at, updated_at, created_by_id, updated_by_id) VALUES (2, 'Editor', 'strapi-editor', 'Editors can manage and publish contents including those of other users.', '2022-12-03 18:53:08.006', '2022-12-03 18:53:08.006', NULL, NULL);
INSERT INTO public.admin_roles (id, name, code, description, created_at, updated_at, created_by_id, updated_by_id) VALUES (3, 'Author', 'strapi-author', 'Authors can manage the content they have created.', '2022-12-03 18:53:08.028', '2022-12-03 18:53:08.028', NULL, NULL);


--
-- TOC entry 3825 (class 0 OID 16405)
-- Dependencies: 202
-- Data for Name: admin_users; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.admin_users (id, firstname, lastname, username, email, password, reset_password_token, registration_token, is_active, blocked, prefered_language, created_at, updated_at, created_by_id, updated_by_id) VALUES (1, 'Admin', 'Admin', NULL, 'admin@admin.com', '$2a$10$o92bvmZBrZnEAJ2lxR99XOTohEIWW09rJv7KHk6MTKe943qwVNcGi', NULL, NULL, true, false, NULL, '2022-12-03 19:07:30.153', '2022-12-03 19:07:30.153', NULL, NULL);


--
-- TOC entry 3827 (class 0 OID 16413)
-- Dependencies: 204
-- Data for Name: admin_users_roles_links; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.admin_users_roles_links (id, user_id, role_id, role_order, user_order) VALUES (1, 1, 1, 1, 1);


--
-- TOC entry 3920 (class 0 OID 214095)
-- Dependencies: 297
-- Data for Name: available_products_pages; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.available_products_pages (id, created_at, updated_at, published_at, created_by_id, updated_by_id, locale) VALUES (2, '2022-12-14 10:15:10.078', '2022-12-14 10:16:43.479', '2022-12-14 10:15:20.092', 1, 1, 'en');
INSERT INTO public.available_products_pages (id, created_at, updated_at, published_at, created_by_id, updated_by_id, locale) VALUES (1, '2022-12-14 10:12:09.988', '2022-12-14 10:16:43.479', '2022-12-14 10:15:49.718', 1, 1, 'de');
INSERT INTO public.available_products_pages (id, created_at, updated_at, published_at, created_by_id, updated_by_id, locale) VALUES (3, '2022-12-14 10:16:43.451', '2022-12-14 10:16:45.72', '2022-12-14 10:16:45.708', 1, 1, 'pl');


--
-- TOC entry 3922 (class 0 OID 214105)
-- Dependencies: 299
-- Data for Name: available_products_pages_components; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.available_products_pages_components (id, entity_id, component_id, component_type, field, "order") VALUES (1, 1, 25, 'shared.seo', 'seo', NULL);
INSERT INTO public.available_products_pages_components (id, entity_id, component_id, component_type, field, "order") VALUES (2, 2, 26, 'shared.seo', 'seo', NULL);
INSERT INTO public.available_products_pages_components (id, entity_id, component_id, component_type, field, "order") VALUES (5, 3, 27, 'shared.seo', 'seo', NULL);


--
-- TOC entry 3924 (class 0 OID 214121)
-- Dependencies: 301
-- Data for Name: available_products_pages_localizations_links; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.available_products_pages_localizations_links (id, available_products_page_id, inv_available_products_page_id, available_products_page_order) VALUES (3, 3, 2, 1);
INSERT INTO public.available_products_pages_localizations_links (id, available_products_page_id, inv_available_products_page_id, available_products_page_order) VALUES (4, 3, 1, 2);
INSERT INTO public.available_products_pages_localizations_links (id, available_products_page_id, inv_available_products_page_id, available_products_page_order) VALUES (5, 2, 3, 1);
INSERT INTO public.available_products_pages_localizations_links (id, available_products_page_id, inv_available_products_page_id, available_products_page_order) VALUES (1, 2, 1, 2);
INSERT INTO public.available_products_pages_localizations_links (id, available_products_page_id, inv_available_products_page_id, available_products_page_order) VALUES (7, 1, 3, 1);
INSERT INTO public.available_products_pages_localizations_links (id, available_products_page_id, inv_available_products_page_id, available_products_page_order) VALUES (2, 1, 2, 2);


--
-- TOC entry 3938 (class 0 OID 214414)
-- Dependencies: 315
-- Data for Name: basket_pages; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.basket_pages (id, created_at, updated_at, published_at, created_by_id, updated_by_id, locale) VALUES (2, '2022-12-14 12:22:34.208', '2022-12-14 12:26:09.922', '2022-12-14 12:22:35.851', 1, 1, 'pl');
INSERT INTO public.basket_pages (id, created_at, updated_at, published_at, created_by_id, updated_by_id, locale) VALUES (1, '2022-12-14 12:22:23.456', '2022-12-14 12:26:42.35', '2022-12-14 12:22:24.902', 1, 1, 'en');
INSERT INTO public.basket_pages (id, created_at, updated_at, published_at, created_by_id, updated_by_id, locale) VALUES (3, '2022-12-14 12:22:52.197', '2022-12-14 12:26:51.746', '2022-12-14 12:22:53.653', 1, 1, 'de');


--
-- TOC entry 3940 (class 0 OID 214424)
-- Dependencies: 317
-- Data for Name: basket_pages_components; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.basket_pages_components (id, entity_id, component_id, component_type, field, "order") VALUES (1, 1, 43, 'shared.seo', 'seo', NULL);
INSERT INTO public.basket_pages_components (id, entity_id, component_id, component_type, field, "order") VALUES (2, 2, 44, 'shared.seo', 'seo', NULL);
INSERT INTO public.basket_pages_components (id, entity_id, component_id, component_type, field, "order") VALUES (3, 3, 45, 'shared.seo', 'seo', NULL);


--
-- TOC entry 3942 (class 0 OID 214440)
-- Dependencies: 319
-- Data for Name: basket_pages_localizations_links; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.basket_pages_localizations_links (id, basket_page_id, inv_basket_page_id, basket_page_order) VALUES (3, 3, 1, 1);
INSERT INTO public.basket_pages_localizations_links (id, basket_page_id, inv_basket_page_id, basket_page_order) VALUES (4, 3, 2, 2);
INSERT INTO public.basket_pages_localizations_links (id, basket_page_id, inv_basket_page_id, basket_page_order) VALUES (5, 1, 3, 1);
INSERT INTO public.basket_pages_localizations_links (id, basket_page_id, inv_basket_page_id, basket_page_order) VALUES (2, 1, 2, 2);
INSERT INTO public.basket_pages_localizations_links (id, basket_page_id, inv_basket_page_id, basket_page_order) VALUES (7, 2, 3, 1);
INSERT INTO public.basket_pages_localizations_links (id, basket_page_id, inv_basket_page_id, basket_page_order) VALUES (1, 2, 1, 2);


--
-- TOC entry 3829 (class 0 OID 16418)
-- Dependencies: 206
-- Data for Name: components_blocks_hero_slider_items; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.components_blocks_hero_slider_items (id, title, text) VALUES (37, 'Suchen Sie beste Ecksofas durch', NULL);
INSERT INTO public.components_blocks_hero_slider_items (id, title, text) VALUES (38, 'Entdecken Sie bequeme Betten', NULL);
INSERT INTO public.components_blocks_hero_slider_items (id, title, text) VALUES (39, 'Kaufen Sie neuste Sessel', NULL);
INSERT INTO public.components_blocks_hero_slider_items (id, title, text) VALUES (40, 'Suchen Sie Sitzgruppen durch', NULL);
INSERT INTO public.components_blocks_hero_slider_items (id, title, text) VALUES (1, 'Browse modern sectionals', NULL);
INSERT INTO public.components_blocks_hero_slider_items (id, title, text) VALUES (2, 'Discover comfortable beds', NULL);
INSERT INTO public.components_blocks_hero_slider_items (id, title, text) VALUES (35, 'Shop best armchairs', NULL);
INSERT INTO public.components_blocks_hero_slider_items (id, title, text) VALUES (36, 'Browse best lounge suites', NULL);
INSERT INTO public.components_blocks_hero_slider_items (id, title, text) VALUES (43, 'Przeglądaj stylowe narożniki', NULL);
INSERT INTO public.components_blocks_hero_slider_items (id, title, text) VALUES (41, 'Odkrywaj komfortowe łóżka', NULL);
INSERT INTO public.components_blocks_hero_slider_items (id, title, text) VALUES (42, 'Kup idealne fotele', NULL);
INSERT INTO public.components_blocks_hero_slider_items (id, title, text) VALUES (44, 'Przeglądaj zestawy meblowe', NULL);


--
-- TOC entry 3830 (class 0 OID 16424)
-- Dependencies: 207
-- Data for Name: components_blocks_hero_slider_items_components; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.components_blocks_hero_slider_items_components (id, entity_id, component_id, component_type, field, "order") VALUES (1, 1, 1, 'shared.link', 'cta', NULL);
INSERT INTO public.components_blocks_hero_slider_items_components (id, entity_id, component_id, component_type, field, "order") VALUES (2, 2, 2, 'shared.link', 'cta', NULL);
INSERT INTO public.components_blocks_hero_slider_items_components (id, entity_id, component_id, component_type, field, "order") VALUES (35, 35, 35, 'shared.link', 'cta', NULL);
INSERT INTO public.components_blocks_hero_slider_items_components (id, entity_id, component_id, component_type, field, "order") VALUES (38, 36, 36, 'shared.link', 'cta', NULL);
INSERT INTO public.components_blocks_hero_slider_items_components (id, entity_id, component_id, component_type, field, "order") VALUES (46, 37, 37, 'shared.link', 'cta', NULL);
INSERT INTO public.components_blocks_hero_slider_items_components (id, entity_id, component_id, component_type, field, "order") VALUES (47, 38, 39, 'shared.link', 'cta', NULL);
INSERT INTO public.components_blocks_hero_slider_items_components (id, entity_id, component_id, component_type, field, "order") VALUES (48, 39, 38, 'shared.link', 'cta', NULL);
INSERT INTO public.components_blocks_hero_slider_items_components (id, entity_id, component_id, component_type, field, "order") VALUES (49, 40, 40, 'shared.link', 'cta', NULL);
INSERT INTO public.components_blocks_hero_slider_items_components (id, entity_id, component_id, component_type, field, "order") VALUES (50, 41, 41, 'shared.link', 'cta', NULL);
INSERT INTO public.components_blocks_hero_slider_items_components (id, entity_id, component_id, component_type, field, "order") VALUES (51, 42, 42, 'shared.link', 'cta', NULL);
INSERT INTO public.components_blocks_hero_slider_items_components (id, entity_id, component_id, component_type, field, "order") VALUES (52, 43, 43, 'shared.link', 'cta', NULL);
INSERT INTO public.components_blocks_hero_slider_items_components (id, entity_id, component_id, component_type, field, "order") VALUES (53, 44, 44, 'shared.link', 'cta', NULL);


--
-- TOC entry 3833 (class 0 OID 16434)
-- Dependencies: 210
-- Data for Name: components_blocks_hero_sliders; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.components_blocks_hero_sliders (id) VALUES (2);
INSERT INTO public.components_blocks_hero_sliders (id) VALUES (1);
INSERT INTO public.components_blocks_hero_sliders (id) VALUES (3);


--
-- TOC entry 3834 (class 0 OID 16437)
-- Dependencies: 211
-- Data for Name: components_blocks_hero_sliders_components; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.components_blocks_hero_sliders_components (id, entity_id, component_id, component_type, field, "order") VALUES (46, 2, 37, 'blocks.hero-slider-item', 'heroSliderItems', 1);
INSERT INTO public.components_blocks_hero_sliders_components (id, entity_id, component_id, component_type, field, "order") VALUES (47, 2, 38, 'blocks.hero-slider-item', 'heroSliderItems', 2);
INSERT INTO public.components_blocks_hero_sliders_components (id, entity_id, component_id, component_type, field, "order") VALUES (48, 2, 39, 'blocks.hero-slider-item', 'heroSliderItems', 3);
INSERT INTO public.components_blocks_hero_sliders_components (id, entity_id, component_id, component_type, field, "order") VALUES (49, 2, 40, 'blocks.hero-slider-item', 'heroSliderItems', 4);
INSERT INTO public.components_blocks_hero_sliders_components (id, entity_id, component_id, component_type, field, "order") VALUES (1, 1, 1, 'blocks.hero-slider-item', 'heroSliderItems', 1);
INSERT INTO public.components_blocks_hero_sliders_components (id, entity_id, component_id, component_type, field, "order") VALUES (3, 1, 2, 'blocks.hero-slider-item', 'heroSliderItems', 2);
INSERT INTO public.components_blocks_hero_sliders_components (id, entity_id, component_id, component_type, field, "order") VALUES (37, 1, 35, 'blocks.hero-slider-item', 'heroSliderItems', 3);
INSERT INTO public.components_blocks_hero_sliders_components (id, entity_id, component_id, component_type, field, "order") VALUES (41, 1, 36, 'blocks.hero-slider-item', 'heroSliderItems', 4);
INSERT INTO public.components_blocks_hero_sliders_components (id, entity_id, component_id, component_type, field, "order") VALUES (50, 3, 43, 'blocks.hero-slider-item', 'heroSliderItems', 1);
INSERT INTO public.components_blocks_hero_sliders_components (id, entity_id, component_id, component_type, field, "order") VALUES (51, 3, 41, 'blocks.hero-slider-item', 'heroSliderItems', 2);
INSERT INTO public.components_blocks_hero_sliders_components (id, entity_id, component_id, component_type, field, "order") VALUES (52, 3, 42, 'blocks.hero-slider-item', 'heroSliderItems', 3);
INSERT INTO public.components_blocks_hero_sliders_components (id, entity_id, component_id, component_type, field, "order") VALUES (53, 3, 44, 'blocks.hero-slider-item', 'heroSliderItems', 4);


--
-- TOC entry 3837 (class 0 OID 16447)
-- Dependencies: 214
-- Data for Name: components_shared_links; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.components_shared_links (id, href, label, target, is_external) VALUES (37, 'de/Products/Category/Index/4d54153b-10a6-4bf7-9a44-a38759d8be53', 'Durchsuchen', NULL, false);
INSERT INTO public.components_shared_links (id, href, label, target, is_external) VALUES (38, 'de/Products/Category/Index/b89c7c6c-a4f2-442b-8813-51c39291aa4e', 'Jetzt einkaufen', NULL, false);
INSERT INTO public.components_shared_links (id, href, label, target, is_external) VALUES (39, 'de/Products/Category/Index/1b4a61fb-cdda-45b2-a4d6-92a27acdf833', 'Entdecken', NULL, false);
INSERT INTO public.components_shared_links (id, href, label, target, is_external) VALUES (40, 'de/Products/Category/Index/c8fc3b79-c17f-4ac8-99a8-e02619ed17ec', 'Durchsuchen', NULL, false);
INSERT INTO public.components_shared_links (id, href, label, target, is_external) VALUES (2, 'en/Products/Category/Index/1b4a61fb-cdda-45b2-a4d6-92a27acdf833', 'Discover', NULL, false);
INSERT INTO public.components_shared_links (id, href, label, target, is_external) VALUES (1, 'en/Products/Category/Index/c8fc3b79-c17f-4ac8-99a8-e02619ed17ec', 'Browse', NULL, false);
INSERT INTO public.components_shared_links (id, href, label, target, is_external) VALUES (35, 'en/Products/Category/Index/b89c7c6c-a4f2-442b-8813-51c39291aa4e', 'Shop now', NULL, false);
INSERT INTO public.components_shared_links (id, href, label, target, is_external) VALUES (36, 'en/Products/Category/Index/c8fc3b79-c17f-4ac8-99a8-e02619ed17ec', 'Browse', NULL, false);
INSERT INTO public.components_shared_links (id, href, label, target, is_external) VALUES (43, 'pl/Products/Category/Index/4d54153b-10a6-4bf7-9a44-a38759d8be53', 'Przeglądaj produkty', NULL, false);
INSERT INTO public.components_shared_links (id, href, label, target, is_external) VALUES (41, 'pl/Products/Category/Index/1b4a61fb-cdda-45b2-a4d6-92a27acdf833', 'Odkrywaj produkty', NULL, false);
INSERT INTO public.components_shared_links (id, href, label, target, is_external) VALUES (42, 'pl/Products/Category/Index/b89c7c6c-a4f2-442b-8813-51c39291aa4e', 'Zapoznaj się z ofertą', NULL, false);
INSERT INTO public.components_shared_links (id, href, label, target, is_external) VALUES (44, 'pl/Products/Category/Index/c8fc3b79-c17f-4ac8-99a8-e02619ed17ec', 'Przeglądaj produkty', NULL, false);


--
-- TOC entry 3839 (class 0 OID 16455)
-- Dependencies: 216
-- Data for Name: components_shared_seos; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (2, 'Hauptseite', 'Beste Möbel in Polen');
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (1, 'Home Page', 'Best furniture in Poland');
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (3, 'Strona główna', 'Najlepsze meble w Polsce');
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (7, 'Armaturenbrett - B2B', NULL);
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (5, 'Dashboard - B2B', NULL);
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (6, 'Panel - B2B', NULL);
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (8, 'Download Center - B2B', NULL);
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (9, 'test', NULL);
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (10, 'Orders', NULL);
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (12, 'Bestellungen', NULL);
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (13, 'Bestellung', NULL);
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (14, 'Order', NULL);
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (15, 'Zamówienie', NULL);
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (16, 'Aktualności', NULL);
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (17, 'News', NULL);
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (18, 'Auslauf', NULL);
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (19, 'Dashboard', NULL);
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (20, 'Panel', NULL);
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (21, 'Armaturenbrett', NULL);
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (22, 'Download Center', NULL);
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (23, 'Download-Center', NULL);
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (24, 'Centrum Pobierania', NULL);
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (25, 'Verfügbare Produkte', NULL);
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (26, 'Available products', NULL);
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (27, 'Stany magazynowe', NULL);
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (28, 'News', NULL);
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (29, 'Outlet', NULL);
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (30, 'Wyprzedaż', NULL);
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (11, 'Zamówienia', NULL);
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (31, 'Regulations', NULL);
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (32, 'Regulamin', NULL);
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (33, 'Geschäftsbedingungen', NULL);
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (34, 'Privacy Policy', NULL);
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (35, 'Polityka Prywatności', NULL);
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (36, 'Datenschutz-Bestimmungen', NULL);
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (37, 'ELTAP. The best furniture in Poland', NULL);
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (38, 'Najlepsze meble w Polsce', NULL);
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (39, 'ELTAP. Die besten Möbel in Polen', NULL);
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (40, 'Bestellungsartikel', NULL);
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (41, 'Order Item', NULL);
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (42, 'Pozycja zamówienia', NULL);
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (44, 'Zamówienie', NULL);
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (43, 'Order', NULL);
INSERT INTO public.components_shared_seos (id, meta_title, meta_description) VALUES (45, 'Bestellung', NULL);


--
-- TOC entry 3884 (class 0 OID 213618)
-- Dependencies: 261
-- Data for Name: dashboard_pages; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.dashboard_pages (id, created_at, updated_at, published_at, created_by_id, updated_by_id, locale) VALUES (2, '2022-12-14 10:08:04.257', '2022-12-14 10:08:21.156', '2022-12-14 10:08:05.958', 1, 1, 'pl');
INSERT INTO public.dashboard_pages (id, created_at, updated_at, published_at, created_by_id, updated_by_id, locale) VALUES (1, '2022-12-14 10:07:54.327', '2022-12-14 10:08:21.156', '2022-12-14 10:07:56.339', 1, 1, 'en');
INSERT INTO public.dashboard_pages (id, created_at, updated_at, published_at, created_by_id, updated_by_id, locale) VALUES (3, '2022-12-14 10:08:21.122', '2022-12-14 10:08:23.189', '2022-12-14 10:08:23.18', 1, 1, 'de');


--
-- TOC entry 3896 (class 0 OID 213688)
-- Dependencies: 273
-- Data for Name: dashboard_pages_components; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.dashboard_pages_components (id, entity_id, component_id, component_type, field, "order") VALUES (1, 1, 19, 'shared.seo', 'seo', NULL);
INSERT INTO public.dashboard_pages_components (id, entity_id, component_id, component_type, field, "order") VALUES (2, 2, 20, 'shared.seo', 'seo', NULL);
INSERT INTO public.dashboard_pages_components (id, entity_id, component_id, component_type, field, "order") VALUES (3, 3, 21, 'shared.seo', 'seo', NULL);


--
-- TOC entry 3898 (class 0 OID 213704)
-- Dependencies: 275
-- Data for Name: dashboard_pages_localizations_links; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.dashboard_pages_localizations_links (id, dashboard_page_id, inv_dashboard_page_id, dashboard_page_order) VALUES (3, 3, 1, 1);
INSERT INTO public.dashboard_pages_localizations_links (id, dashboard_page_id, inv_dashboard_page_id, dashboard_page_order) VALUES (4, 3, 2, 2);
INSERT INTO public.dashboard_pages_localizations_links (id, dashboard_page_id, inv_dashboard_page_id, dashboard_page_order) VALUES (5, 2, 3, 1);
INSERT INTO public.dashboard_pages_localizations_links (id, dashboard_page_id, inv_dashboard_page_id, dashboard_page_order) VALUES (1, 2, 1, 2);
INSERT INTO public.dashboard_pages_localizations_links (id, dashboard_page_id, inv_dashboard_page_id, dashboard_page_order) VALUES (7, 1, 3, 1);
INSERT INTO public.dashboard_pages_localizations_links (id, dashboard_page_id, inv_dashboard_page_id, dashboard_page_order) VALUES (2, 1, 2, 2);


--
-- TOC entry 3886 (class 0 OID 213628)
-- Dependencies: 263
-- Data for Name: download_center_pages; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.download_center_pages (id, created_at, updated_at, published_at, created_by_id, updated_by_id, locale) VALUES (1, '2022-12-14 10:09:00.96', '2022-12-14 10:09:26.252', '2022-12-14 10:09:03.137', 1, 1, 'en');
INSERT INTO public.download_center_pages (id, created_at, updated_at, published_at, created_by_id, updated_by_id, locale) VALUES (2, '2022-12-14 10:09:12.5', '2022-12-14 10:09:26.252', '2022-12-14 10:09:14.831', 1, 1, 'de');
INSERT INTO public.download_center_pages (id, created_at, updated_at, published_at, created_by_id, updated_by_id, locale) VALUES (3, '2022-12-14 10:09:26.212', '2022-12-14 10:09:27.963', '2022-12-14 10:09:27.952', 1, 1, 'pl');


--
-- TOC entry 3900 (class 0 OID 213717)
-- Dependencies: 277
-- Data for Name: download_center_pages_components; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.download_center_pages_components (id, entity_id, component_id, component_type, field, "order") VALUES (1, 1, 22, 'shared.seo', 'seo', NULL);
INSERT INTO public.download_center_pages_components (id, entity_id, component_id, component_type, field, "order") VALUES (2, 2, 23, 'shared.seo', 'seo', NULL);
INSERT INTO public.download_center_pages_components (id, entity_id, component_id, component_type, field, "order") VALUES (3, 3, 24, 'shared.seo', 'seo', NULL);


--
-- TOC entry 3902 (class 0 OID 213733)
-- Dependencies: 279
-- Data for Name: download_center_pages_localizations_links; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.download_center_pages_localizations_links (id, download_center_page_id, inv_download_center_page_id, download_center_page_order) VALUES (3, 3, 1, 1);
INSERT INTO public.download_center_pages_localizations_links (id, download_center_page_id, inv_download_center_page_id, download_center_page_order) VALUES (4, 3, 2, 2);
INSERT INTO public.download_center_pages_localizations_links (id, download_center_page_id, inv_download_center_page_id, download_center_page_order) VALUES (5, 1, 3, 1);
INSERT INTO public.download_center_pages_localizations_links (id, download_center_page_id, inv_download_center_page_id, download_center_page_order) VALUES (2, 1, 2, 2);
INSERT INTO public.download_center_pages_localizations_links (id, download_center_page_id, inv_download_center_page_id, download_center_page_order) VALUES (7, 2, 3, 1);
INSERT INTO public.download_center_pages_localizations_links (id, download_center_page_id, inv_download_center_page_id, download_center_page_order) VALUES (1, 2, 1, 2);


--
-- TOC entry 3841 (class 0 OID 16463)
-- Dependencies: 218
-- Data for Name: files; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.files (id, name, alternative_text, caption, width, height, formats, hash, ext, mime, size, url, preview_url, provider, provider_metadata, folder_path, created_at, updated_at, created_by_id, updated_by_id) VALUES (2, 'Boxsprings.jpg', NULL, NULL, 1920, 400, '{"thumbnail": {"ext": ".jpg", "url": "http://host.docker.internal:5131/api/v1/files/21513405-c949-4373-638b-08dad6fb1755", "hash": "thumbnail_Boxsprings_3e82841063", "mime": "image/jpeg", "name": "thumbnail_Boxsprings.jpg", "path": "http://host.docker.internal:5131/api/v1/files/21513405-c949-4373-638b-08dad6fb1755", "size": 3.54, "width": 245, "height": 51}}', 'Boxsprings_3e82841063', '.jpg', 'image/jpeg', 412.77, 'http://host.docker.internal:5131/api/v1/files/3d206600-4d0d-4bb0-880a-90ba85bbaeb5', NULL, 'giuru', NULL, '/', '2022-12-05 19:58:34.579', '2022-12-05 19:58:34.579', 1, 1);
INSERT INTO public.files (id, name, alternative_text, caption, width, height, formats, hash, ext, mime, size, url, preview_url, provider, provider_metadata, folder_path, created_at, updated_at, created_by_id, updated_by_id) VALUES (35, 'Chairs.jpg', NULL, NULL, 1920, 400, '{"thumbnail": {"ext": ".jpg", "url": "http://host.docker.internal:5131/api/v1/files/b0447096-c28f-4b43-418c-08dad6fba714", "hash": "thumbnail_Chairs_a6e41e5455", "mime": "image/jpeg", "name": "thumbnail_Chairs.jpg", "path": "http://host.docker.internal:5131/api/v1/files/b0447096-c28f-4b43-418c-08dad6fba714", "size": 3.51, "width": 245, "height": 51}}', 'Chairs_a6e41e5455', '.jpg', 'image/jpeg', 369.34, 'http://host.docker.internal:5131/api/v1/files/09e85fba-f63a-45e2-87ab-12e3b78ed650', NULL, 'giuru', NULL, '/', '2022-12-05 20:02:35.686', '2022-12-05 20:02:35.686', 1, 1);
INSERT INTO public.files (id, name, alternative_text, caption, width, height, formats, hash, ext, mime, size, url, preview_url, provider, provider_metadata, folder_path, created_at, updated_at, created_by_id, updated_by_id) VALUES (1, 'Corners.jpg', 'Corners', 'Corners', 1920, 400, '{"thumbnail": {"ext": ".jpg", "url": "http://host.docker.internal:5131/api/v1/files/d38e121d-2894-424f-2561-08dad632acc2", "hash": "thumbnail_Corners_8db064ba69", "mime": "image/jpeg", "name": "thumbnail_Corners.jpg", "path": "http://host.docker.internal:5131/api/v1/files/d38e121d-2894-424f-2561-08dad632acc2", "size": 3.45, "width": 245, "height": 51}}', 'Corners_8db064ba69', '.jpg', 'image/jpeg', 427.82, 'http://host.docker.internal:5131/api/v1/files/19221400-3812-49b6-a8d7-fb8bc8223919', NULL, 'giuru', NULL, '/', '2022-12-04 20:03:56.224', '2022-12-05 20:08:13.66', 1, 1);
INSERT INTO public.files (id, name, alternative_text, caption, width, height, formats, hash, ext, mime, size, url, preview_url, provider, provider_metadata, folder_path, created_at, updated_at, created_by_id, updated_by_id) VALUES (36, 'Sets.jpg', NULL, NULL, 1920, 400, '{"thumbnail": {"ext": ".jpg", "url": "http://host.docker.internal:5131/api/v1/files/f9098ca7-75a8-4241-418d-08dad6fba714", "hash": "thumbnail_Sets_206b71cb19", "mime": "image/jpeg", "name": "thumbnail_Sets.jpg", "path": "http://host.docker.internal:5131/api/v1/files/f9098ca7-75a8-4241-418d-08dad6fba714", "size": 3.53, "width": 245, "height": 51}}', 'Sets_206b71cb19', '.jpg', 'image/jpeg', 160.13, 'http://host.docker.internal:5131/api/v1/files/4c299e20-c8c5-47e1-bbe3-c308e8bd4478', NULL, 'giuru', NULL, '/', '2022-12-05 20:04:23.445', '2022-12-05 20:15:11.906', 1, 1);


--
-- TOC entry 3842 (class 0 OID 16469)
-- Dependencies: 219
-- Data for Name: files_folder_links; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 3845 (class 0 OID 16476)
-- Dependencies: 222
-- Data for Name: files_related_morphs; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.files_related_morphs (id, file_id, related_id, related_type, field, "order") VALUES (46, 1, 37, 'blocks.hero-slider-item', 'image', 1);
INSERT INTO public.files_related_morphs (id, file_id, related_id, related_type, field, "order") VALUES (47, 2, 38, 'blocks.hero-slider-item', 'image', 1);
INSERT INTO public.files_related_morphs (id, file_id, related_id, related_type, field, "order") VALUES (48, 35, 39, 'blocks.hero-slider-item', 'image', 1);
INSERT INTO public.files_related_morphs (id, file_id, related_id, related_type, field, "order") VALUES (49, 36, 40, 'blocks.hero-slider-item', 'image', 1);
INSERT INTO public.files_related_morphs (id, file_id, related_id, related_type, field, "order") VALUES (59, 1, 1, 'blocks.hero-slider-item', 'image', 1);
INSERT INTO public.files_related_morphs (id, file_id, related_id, related_type, field, "order") VALUES (60, 2, 2, 'blocks.hero-slider-item', 'image', 1);
INSERT INTO public.files_related_morphs (id, file_id, related_id, related_type, field, "order") VALUES (61, 35, 35, 'blocks.hero-slider-item', 'image', 1);
INSERT INTO public.files_related_morphs (id, file_id, related_id, related_type, field, "order") VALUES (62, 36, 36, 'blocks.hero-slider-item', 'image', 1);
INSERT INTO public.files_related_morphs (id, file_id, related_id, related_type, field, "order") VALUES (67, 1, 43, 'blocks.hero-slider-item', 'image', 1);
INSERT INTO public.files_related_morphs (id, file_id, related_id, related_type, field, "order") VALUES (68, 2, 41, 'blocks.hero-slider-item', 'image', 1);
INSERT INTO public.files_related_morphs (id, file_id, related_id, related_type, field, "order") VALUES (69, 35, 42, 'blocks.hero-slider-item', 'image', 1);
INSERT INTO public.files_related_morphs (id, file_id, related_id, related_type, field, "order") VALUES (70, 36, 44, 'blocks.hero-slider-item', 'image', 1);


--
-- TOC entry 3847 (class 0 OID 16484)
-- Dependencies: 224
-- Data for Name: home_pages; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.home_pages (id, created_at, updated_at, published_at, created_by_id, updated_by_id, locale) VALUES (2, '2022-12-05 20:11:41.797', '2022-12-05 20:15:20.494', '2022-12-05 20:11:44.733', 1, 1, 'de');
INSERT INTO public.home_pages (id, created_at, updated_at, published_at, created_by_id, updated_by_id, locale) VALUES (1, '2022-12-04 20:06:54.7', '2022-12-06 06:12:31.557', '2022-12-04 20:22:36.976', 1, 1, 'en');
INSERT INTO public.home_pages (id, created_at, updated_at, published_at, created_by_id, updated_by_id, locale) VALUES (3, '2022-12-05 20:15:20.455', '2022-12-08 07:40:28.332', '2022-12-05 20:15:22.28', 1, 1, 'pl');


--
-- TOC entry 3848 (class 0 OID 16487)
-- Dependencies: 225
-- Data for Name: home_pages_components; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.home_pages_components (id, entity_id, component_id, component_type, field, "order") VALUES (1, 1, 1, 'blocks.hero-slider', 'heroSlider', NULL);
INSERT INTO public.home_pages_components (id, entity_id, component_id, component_type, field, "order") VALUES (37, 1, 1, 'shared.seo', 'seo', NULL);
INSERT INTO public.home_pages_components (id, entity_id, component_id, component_type, field, "order") VALUES (39, 2, 2, 'shared.seo', 'seo', NULL);
INSERT INTO public.home_pages_components (id, entity_id, component_id, component_type, field, "order") VALUES (40, 2, 2, 'blocks.hero-slider', 'heroSlider', NULL);
INSERT INTO public.home_pages_components (id, entity_id, component_id, component_type, field, "order") VALUES (41, 3, 3, 'shared.seo', 'seo', NULL);
INSERT INTO public.home_pages_components (id, entity_id, component_id, component_type, field, "order") VALUES (42, 3, 3, 'blocks.hero-slider', 'heroSlider', NULL);


--
-- TOC entry 3851 (class 0 OID 16497)
-- Dependencies: 228
-- Data for Name: home_pages_localizations_links; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.home_pages_localizations_links (id, home_page_id, inv_home_page_id, home_page_order) VALUES (3, 3, 1, 1);
INSERT INTO public.home_pages_localizations_links (id, home_page_id, inv_home_page_id, home_page_order) VALUES (4, 3, 2, 2);
INSERT INTO public.home_pages_localizations_links (id, home_page_id, inv_home_page_id, home_page_order) VALUES (5, 1, 3, 1);
INSERT INTO public.home_pages_localizations_links (id, home_page_id, inv_home_page_id, home_page_order) VALUES (2, 1, 2, 2);
INSERT INTO public.home_pages_localizations_links (id, home_page_id, inv_home_page_id, home_page_order) VALUES (7, 2, 3, 1);
INSERT INTO public.home_pages_localizations_links (id, home_page_id, inv_home_page_id, home_page_order) VALUES (1, 2, 1, 2);


--
-- TOC entry 3853 (class 0 OID 16502)
-- Dependencies: 230
-- Data for Name: i18n_locale; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.i18n_locale (id, name, code, created_at, updated_at, created_by_id, updated_by_id) VALUES (1, 'English (en)', 'en', '2022-12-03 18:53:07.881', '2022-12-03 18:53:07.881', NULL, NULL);
INSERT INTO public.i18n_locale (id, name, code, created_at, updated_at, created_by_id, updated_by_id) VALUES (2, 'Polish (pl)', 'pl', '2022-12-03 21:47:30.412', '2022-12-03 21:47:30.412', 1, 1);
INSERT INTO public.i18n_locale (id, name, code, created_at, updated_at, created_by_id, updated_by_id) VALUES (3, 'German (de)', 'de', '2022-12-03 21:47:40.418', '2022-12-03 21:47:40.418', 1, 1);


--
-- TOC entry 3888 (class 0 OID 213648)
-- Dependencies: 265
-- Data for Name: news_pages; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.news_pages (id, created_at, updated_at, published_at, created_by_id, updated_by_id, locale) VALUES (3, '2022-12-14 10:17:23.582', '2022-12-14 10:17:25.796', '2022-12-14 10:17:25.786', 1, 1, 'en');
INSERT INTO public.news_pages (id, created_at, updated_at, published_at, created_by_id, updated_by_id, locale) VALUES (2, '2022-12-14 10:05:36.238', '2022-12-14 10:17:29.802', '2022-12-14 10:17:29.792', 1, 1, 'de');
INSERT INTO public.news_pages (id, created_at, updated_at, published_at, created_by_id, updated_by_id, locale) VALUES (1, '2022-12-14 10:04:45.445', '2022-12-14 10:17:34.872', '2022-12-14 10:17:34.863', 1, 1, 'pl');


--
-- TOC entry 3904 (class 0 OID 213775)
-- Dependencies: 281
-- Data for Name: news_pages_components; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.news_pages_components (id, entity_id, component_id, component_type, field, "order") VALUES (1, 1, 16, 'shared.seo', 'seo', NULL);
INSERT INTO public.news_pages_components (id, entity_id, component_id, component_type, field, "order") VALUES (2, 2, 17, 'shared.seo', 'seo', NULL);
INSERT INTO public.news_pages_components (id, entity_id, component_id, component_type, field, "order") VALUES (3, 3, 28, 'shared.seo', 'seo', NULL);


--
-- TOC entry 3906 (class 0 OID 213791)
-- Dependencies: 283
-- Data for Name: news_pages_localizations_links; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.news_pages_localizations_links (id, news_page_id, inv_news_page_id, news_page_order) VALUES (3, 3, 2, 1);
INSERT INTO public.news_pages_localizations_links (id, news_page_id, inv_news_page_id, news_page_order) VALUES (4, 3, 1, 2);
INSERT INTO public.news_pages_localizations_links (id, news_page_id, inv_news_page_id, news_page_order) VALUES (5, 1, 3, 1);
INSERT INTO public.news_pages_localizations_links (id, news_page_id, inv_news_page_id, news_page_order) VALUES (2, 1, 2, 2);
INSERT INTO public.news_pages_localizations_links (id, news_page_id, inv_news_page_id, news_page_order) VALUES (7, 2, 3, 1);
INSERT INTO public.news_pages_localizations_links (id, news_page_id, inv_news_page_id, news_page_order) VALUES (1, 2, 1, 2);


--
-- TOC entry 3944 (class 0 OID 214505)
-- Dependencies: 321
-- Data for Name: order_item_pages; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.order_item_pages (id, created_at, updated_at, published_at, created_by_id, updated_by_id, locale) VALUES (2, '2022-12-14 12:14:59.209', '2022-12-14 12:15:15.988', '2022-12-14 12:15:00.897', 1, 1, 'en');
INSERT INTO public.order_item_pages (id, created_at, updated_at, published_at, created_by_id, updated_by_id, locale) VALUES (1, '2022-12-14 12:13:27.063', '2022-12-14 12:15:15.988', '2022-12-14 12:15:04.441', 1, 1, 'de');
INSERT INTO public.order_item_pages (id, created_at, updated_at, published_at, created_by_id, updated_by_id, locale) VALUES (3, '2022-12-14 12:15:15.954', '2022-12-14 12:15:17.761', '2022-12-14 12:15:17.748', 1, 1, 'pl');


--
-- TOC entry 3946 (class 0 OID 214515)
-- Dependencies: 323
-- Data for Name: order_item_pages_components; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.order_item_pages_components (id, entity_id, component_id, component_type, field, "order") VALUES (1, 1, 40, 'shared.seo', 'seo', NULL);
INSERT INTO public.order_item_pages_components (id, entity_id, component_id, component_type, field, "order") VALUES (2, 2, 41, 'shared.seo', 'seo', NULL);
INSERT INTO public.order_item_pages_components (id, entity_id, component_id, component_type, field, "order") VALUES (3, 3, 42, 'shared.seo', 'seo', NULL);


--
-- TOC entry 3948 (class 0 OID 214531)
-- Dependencies: 325
-- Data for Name: order_item_pages_localizations_links; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.order_item_pages_localizations_links (id, order_item_page_id, inv_order_item_page_id, order_item_page_order) VALUES (3, 3, 2, 1);
INSERT INTO public.order_item_pages_localizations_links (id, order_item_page_id, inv_order_item_page_id, order_item_page_order) VALUES (4, 3, 1, 2);
INSERT INTO public.order_item_pages_localizations_links (id, order_item_page_id, inv_order_item_page_id, order_item_page_order) VALUES (5, 1, 3, 1);
INSERT INTO public.order_item_pages_localizations_links (id, order_item_page_id, inv_order_item_page_id, order_item_page_order) VALUES (2, 1, 2, 2);
INSERT INTO public.order_item_pages_localizations_links (id, order_item_page_id, inv_order_item_page_id, order_item_page_order) VALUES (7, 2, 3, 1);
INSERT INTO public.order_item_pages_localizations_links (id, order_item_page_id, inv_order_item_page_id, order_item_page_order) VALUES (1, 2, 1, 2);


--
-- TOC entry 3890 (class 0 OID 213658)
-- Dependencies: 267
-- Data for Name: order_pages; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.order_pages (id, created_at, updated_at, published_at, created_by_id, updated_by_id, locale) VALUES (2, '2022-12-14 10:02:42.328', '2022-12-14 10:03:10.443', '2022-12-14 10:02:46.48', 1, 1, 'en');
INSERT INTO public.order_pages (id, created_at, updated_at, published_at, created_by_id, updated_by_id, locale) VALUES (1, '2022-12-14 10:02:25.473', '2022-12-14 10:03:10.443', '2022-12-14 10:02:51.896', 1, 1, 'de');
INSERT INTO public.order_pages (id, created_at, updated_at, published_at, created_by_id, updated_by_id, locale) VALUES (3, '2022-12-14 10:03:10.419', '2022-12-14 10:03:12.369', '2022-12-14 10:03:12.36', 1, 1, 'pl');


--
-- TOC entry 3908 (class 0 OID 213804)
-- Dependencies: 285
-- Data for Name: order_pages_components; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.order_pages_components (id, entity_id, component_id, component_type, field, "order") VALUES (1, 1, 13, 'shared.seo', 'seo', NULL);
INSERT INTO public.order_pages_components (id, entity_id, component_id, component_type, field, "order") VALUES (2, 2, 14, 'shared.seo', 'seo', NULL);
INSERT INTO public.order_pages_components (id, entity_id, component_id, component_type, field, "order") VALUES (3, 3, 15, 'shared.seo', 'seo', NULL);


--
-- TOC entry 3910 (class 0 OID 213820)
-- Dependencies: 287
-- Data for Name: order_pages_localizations_links; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.order_pages_localizations_links (id, order_page_id, inv_order_page_id, order_page_order) VALUES (3, 3, 2, 1);
INSERT INTO public.order_pages_localizations_links (id, order_page_id, inv_order_page_id, order_page_order) VALUES (4, 3, 1, 2);
INSERT INTO public.order_pages_localizations_links (id, order_page_id, inv_order_page_id, order_page_order) VALUES (5, 2, 3, 1);
INSERT INTO public.order_pages_localizations_links (id, order_page_id, inv_order_page_id, order_page_order) VALUES (1, 2, 1, 2);
INSERT INTO public.order_pages_localizations_links (id, order_page_id, inv_order_page_id, order_page_order) VALUES (7, 1, 3, 1);
INSERT INTO public.order_pages_localizations_links (id, order_page_id, inv_order_page_id, order_page_order) VALUES (2, 1, 2, 2);


--
-- TOC entry 3892 (class 0 OID 213668)
-- Dependencies: 269
-- Data for Name: orders_pages; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.orders_pages (id, created_at, updated_at, published_at, created_by_id, updated_by_id, locale) VALUES (1, '2022-12-14 09:29:19.121', '2022-12-14 10:03:15.819', '2022-12-14 10:03:15.809', 1, 1, 'en');
INSERT INTO public.orders_pages (id, created_at, updated_at, published_at, created_by_id, updated_by_id, locale) VALUES (3, '2022-12-14 09:29:48.747', '2022-12-14 10:03:20.909', '2022-12-14 10:03:20.899', 1, 1, 'de');
INSERT INTO public.orders_pages (id, created_at, updated_at, published_at, created_by_id, updated_by_id, locale) VALUES (2, '2022-12-14 09:29:31.849', '2022-12-14 11:00:13.534', '2022-12-14 10:03:18.284', 1, 1, 'pl');


--
-- TOC entry 3912 (class 0 OID 213833)
-- Dependencies: 289
-- Data for Name: orders_pages_components; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.orders_pages_components (id, entity_id, component_id, component_type, field, "order") VALUES (1, 1, 10, 'shared.seo', 'seo', NULL);
INSERT INTO public.orders_pages_components (id, entity_id, component_id, component_type, field, "order") VALUES (2, 2, 11, 'shared.seo', 'seo', NULL);
INSERT INTO public.orders_pages_components (id, entity_id, component_id, component_type, field, "order") VALUES (3, 3, 12, 'shared.seo', 'seo', NULL);


--
-- TOC entry 3914 (class 0 OID 213849)
-- Dependencies: 291
-- Data for Name: orders_pages_localizations_links; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.orders_pages_localizations_links (id, orders_page_id, inv_orders_page_id, orders_page_order) VALUES (3, 3, 2, 1);
INSERT INTO public.orders_pages_localizations_links (id, orders_page_id, inv_orders_page_id, orders_page_order) VALUES (4, 3, 1, 2);
INSERT INTO public.orders_pages_localizations_links (id, orders_page_id, inv_orders_page_id, orders_page_order) VALUES (5, 2, 3, 1);
INSERT INTO public.orders_pages_localizations_links (id, orders_page_id, inv_orders_page_id, orders_page_order) VALUES (1, 2, 1, 2);
INSERT INTO public.orders_pages_localizations_links (id, orders_page_id, inv_orders_page_id, orders_page_order) VALUES (7, 1, 3, 1);
INSERT INTO public.orders_pages_localizations_links (id, orders_page_id, inv_orders_page_id, orders_page_order) VALUES (2, 1, 2, 2);


--
-- TOC entry 3894 (class 0 OID 213678)
-- Dependencies: 271
-- Data for Name: outlet_pages; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.outlet_pages (id, created_at, updated_at, published_at, created_by_id, updated_by_id, locale) VALUES (2, '2022-12-14 10:18:03.733', '2022-12-14 10:18:18.468', NULL, 1, 1, 'en');
INSERT INTO public.outlet_pages (id, created_at, updated_at, published_at, created_by_id, updated_by_id, locale) VALUES (1, '2022-12-14 10:06:11.83', '2022-12-14 10:18:18.468', NULL, 1, 1, 'de');
INSERT INTO public.outlet_pages (id, created_at, updated_at, published_at, created_by_id, updated_by_id, locale) VALUES (3, '2022-12-14 10:18:18.437', '2022-12-14 10:18:21.113', '2022-12-14 10:18:21.103', 1, 1, 'pl');


--
-- TOC entry 3916 (class 0 OID 213862)
-- Dependencies: 293
-- Data for Name: outlet_pages_components; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.outlet_pages_components (id, entity_id, component_id, component_type, field, "order") VALUES (1, 1, 18, 'shared.seo', 'seo', NULL);
INSERT INTO public.outlet_pages_components (id, entity_id, component_id, component_type, field, "order") VALUES (2, 2, 29, 'shared.seo', 'seo', NULL);
INSERT INTO public.outlet_pages_components (id, entity_id, component_id, component_type, field, "order") VALUES (3, 3, 30, 'shared.seo', 'seo', NULL);


--
-- TOC entry 3918 (class 0 OID 213878)
-- Dependencies: 295
-- Data for Name: outlet_pages_localizations_links; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.outlet_pages_localizations_links (id, outlet_page_id, inv_outlet_page_id, outlet_page_order) VALUES (3, 3, 2, 1);
INSERT INTO public.outlet_pages_localizations_links (id, outlet_page_id, inv_outlet_page_id, outlet_page_order) VALUES (4, 3, 1, 2);
INSERT INTO public.outlet_pages_localizations_links (id, outlet_page_id, inv_outlet_page_id, outlet_page_order) VALUES (5, 2, 3, 1);
INSERT INTO public.outlet_pages_localizations_links (id, outlet_page_id, inv_outlet_page_id, outlet_page_order) VALUES (1, 2, 1, 2);
INSERT INTO public.outlet_pages_localizations_links (id, outlet_page_id, inv_outlet_page_id, outlet_page_order) VALUES (7, 1, 3, 1);
INSERT INTO public.outlet_pages_localizations_links (id, outlet_page_id, inv_outlet_page_id, outlet_page_order) VALUES (2, 1, 2, 2);


--
-- TOC entry 3932 (class 0 OID 214271)
-- Dependencies: 309
-- Data for Name: privacy_policy_pages; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.privacy_policy_pages (id, created_at, updated_at, published_at, created_by_id, updated_by_id, locale, text) VALUES (1, '2022-12-14 12:07:36.519', '2022-12-14 12:08:10.328', '2022-12-14 12:07:38.401', 1, 1, 'en', '# Privacy Policy');
INSERT INTO public.privacy_policy_pages (id, created_at, updated_at, published_at, created_by_id, updated_by_id, locale, text) VALUES (2, '2022-12-14 12:07:56.861', '2022-12-14 12:08:10.329', '2022-12-14 12:07:58.503', 1, 1, 'pl', '# Polityka Prywatności');
INSERT INTO public.privacy_policy_pages (id, created_at, updated_at, published_at, created_by_id, updated_by_id, locale, text) VALUES (3, '2022-12-14 12:08:10.3', '2022-12-14 12:08:12.032', '2022-12-14 12:08:12.022', 1, 1, 'de', '# Datenschutz-Bestimmungen');


--
-- TOC entry 3934 (class 0 OID 214284)
-- Dependencies: 311
-- Data for Name: privacy_policy_pages_components; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.privacy_policy_pages_components (id, entity_id, component_id, component_type, field, "order") VALUES (1, 1, 34, 'shared.seo', 'seo', NULL);
INSERT INTO public.privacy_policy_pages_components (id, entity_id, component_id, component_type, field, "order") VALUES (2, 2, 35, 'shared.seo', 'seo', NULL);
INSERT INTO public.privacy_policy_pages_components (id, entity_id, component_id, component_type, field, "order") VALUES (3, 3, 36, 'shared.seo', 'seo', NULL);


--
-- TOC entry 3936 (class 0 OID 214300)
-- Dependencies: 313
-- Data for Name: privacy_policy_pages_localizations_links; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.privacy_policy_pages_localizations_links (id, privacy_policy_page_id, inv_privacy_policy_page_id, privacy_policy_page_order) VALUES (3, 3, 1, 1);
INSERT INTO public.privacy_policy_pages_localizations_links (id, privacy_policy_page_id, inv_privacy_policy_page_id, privacy_policy_page_order) VALUES (4, 3, 2, 2);
INSERT INTO public.privacy_policy_pages_localizations_links (id, privacy_policy_page_id, inv_privacy_policy_page_id, privacy_policy_page_order) VALUES (5, 1, 3, 1);
INSERT INTO public.privacy_policy_pages_localizations_links (id, privacy_policy_page_id, inv_privacy_policy_page_id, privacy_policy_page_order) VALUES (2, 1, 2, 2);
INSERT INTO public.privacy_policy_pages_localizations_links (id, privacy_policy_page_id, inv_privacy_policy_page_id, privacy_policy_page_order) VALUES (7, 2, 3, 1);
INSERT INTO public.privacy_policy_pages_localizations_links (id, privacy_policy_page_id, inv_privacy_policy_page_id, privacy_policy_page_order) VALUES (1, 2, 1, 2);


--
-- TOC entry 3926 (class 0 OID 214181)
-- Dependencies: 303
-- Data for Name: regulations_pages; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.regulations_pages (id, created_at, updated_at, published_at, created_by_id, updated_by_id, locale, text) VALUES (1, '2022-12-14 12:04:18.001', '2022-12-14 12:06:21.736', '2022-12-14 12:04:21.537', 1, 1, 'en', '# Regulations');
INSERT INTO public.regulations_pages (id, created_at, updated_at, published_at, created_by_id, updated_by_id, locale, text) VALUES (3, '2022-12-14 12:06:21.708', '2022-12-14 12:06:23.186', '2022-12-14 12:06:23.173', 1, 1, 'de', '# Geschäftsbedingungen');
INSERT INTO public.regulations_pages (id, created_at, updated_at, published_at, created_by_id, updated_by_id, locale, text) VALUES (2, '2022-12-14 12:05:36.903', '2022-12-14 12:06:27.544', '2022-12-14 12:06:27.531', 1, 1, 'pl', '# Regulamin');


--
-- TOC entry 3928 (class 0 OID 214194)
-- Dependencies: 305
-- Data for Name: regulations_pages_components; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.regulations_pages_components (id, entity_id, component_id, component_type, field, "order") VALUES (1, 1, 31, 'shared.seo', 'seo', NULL);
INSERT INTO public.regulations_pages_components (id, entity_id, component_id, component_type, field, "order") VALUES (3, 2, 32, 'shared.seo', 'seo', NULL);
INSERT INTO public.regulations_pages_components (id, entity_id, component_id, component_type, field, "order") VALUES (4, 3, 33, 'shared.seo', 'seo', NULL);


--
-- TOC entry 3930 (class 0 OID 214210)
-- Dependencies: 307
-- Data for Name: regulations_pages_localizations_links; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.regulations_pages_localizations_links (id, regulations_page_id, inv_regulations_page_id, regulations_page_order) VALUES (3, 3, 2, 1);
INSERT INTO public.regulations_pages_localizations_links (id, regulations_page_id, inv_regulations_page_id, regulations_page_order) VALUES (4, 3, 1, 2);
INSERT INTO public.regulations_pages_localizations_links (id, regulations_page_id, inv_regulations_page_id, regulations_page_order) VALUES (5, 1, 3, 1);
INSERT INTO public.regulations_pages_localizations_links (id, regulations_page_id, inv_regulations_page_id, regulations_page_order) VALUES (2, 1, 2, 2);
INSERT INTO public.regulations_pages_localizations_links (id, regulations_page_id, inv_regulations_page_id, regulations_page_order) VALUES (7, 2, 3, 1);
INSERT INTO public.regulations_pages_localizations_links (id, regulations_page_id, inv_regulations_page_id, regulations_page_order) VALUES (1, 2, 1, 2);


--
-- TOC entry 3950 (class 0 OID 214597)
-- Dependencies: 327
-- Data for Name: search_products_pages; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.search_products_pages (id, created_at, updated_at, published_at, created_by_id, updated_by_id, locale) VALUES (1, '2022-12-14 12:11:20.556', '2022-12-14 12:12:37.548', '2022-12-14 12:11:22.242', 1, 1, 'en');
INSERT INTO public.search_products_pages (id, created_at, updated_at, published_at, created_by_id, updated_by_id, locale) VALUES (2, '2022-12-14 12:11:35.415', '2022-12-14 12:12:37.548', '2022-12-14 12:11:36.702', 1, 1, 'pl');
INSERT INTO public.search_products_pages (id, created_at, updated_at, published_at, created_by_id, updated_by_id, locale) VALUES (3, '2022-12-14 12:12:37.508', '2022-12-14 12:12:39.476', '2022-12-14 12:12:39.448', 1, 1, 'de');


--
-- TOC entry 3952 (class 0 OID 214607)
-- Dependencies: 329
-- Data for Name: search_products_pages_components; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.search_products_pages_components (id, entity_id, component_id, component_type, field, "order") VALUES (1, 1, 37, 'shared.seo', 'seo', NULL);
INSERT INTO public.search_products_pages_components (id, entity_id, component_id, component_type, field, "order") VALUES (2, 2, 38, 'shared.seo', 'seo', NULL);
INSERT INTO public.search_products_pages_components (id, entity_id, component_id, component_type, field, "order") VALUES (3, 3, 39, 'shared.seo', 'seo', NULL);


--
-- TOC entry 3954 (class 0 OID 214623)
-- Dependencies: 331
-- Data for Name: search_products_pages_localizations_links; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.search_products_pages_localizations_links (id, search_products_page_id, inv_search_products_page_id, search_products_page_order) VALUES (3, 3, 1, 1);
INSERT INTO public.search_products_pages_localizations_links (id, search_products_page_id, inv_search_products_page_id, search_products_page_order) VALUES (4, 3, 2, 2);
INSERT INTO public.search_products_pages_localizations_links (id, search_products_page_id, inv_search_products_page_id, search_products_page_order) VALUES (5, 2, 3, 1);
INSERT INTO public.search_products_pages_localizations_links (id, search_products_page_id, inv_search_products_page_id, search_products_page_order) VALUES (1, 2, 1, 2);
INSERT INTO public.search_products_pages_localizations_links (id, search_products_page_id, inv_search_products_page_id, search_products_page_order) VALUES (7, 1, 3, 1);
INSERT INTO public.search_products_pages_localizations_links (id, search_products_page_id, inv_search_products_page_id, search_products_page_order) VALUES (2, 1, 2, 2);


--
-- TOC entry 3855 (class 0 OID 16510)
-- Dependencies: 232
-- Data for Name: strapi_api_token_permissions; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 3857 (class 0 OID 16515)
-- Dependencies: 234
-- Data for Name: strapi_api_token_permissions_token_links; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 3859 (class 0 OID 16520)
-- Dependencies: 236
-- Data for Name: strapi_api_tokens; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.strapi_api_tokens (id, name, description, type, access_key, last_used_at, expires_at, lifespan, created_at, updated_at, created_by_id, updated_by_id) VALUES (1, 'GraphQL', '', 'full-access', '5ba877d48f81f3a68d0acd169cde9c3b37c2e63de80789ddbdf217e3732453253c8c0a37c3bd6a089b1ce114143012e399c3656f64792502676ca4c44ba77e83', '2022-12-14 12:27:51.885', NULL, NULL, '2022-12-04 11:02:42.763', '2022-12-14 12:27:51.885', NULL, NULL);


--
-- TOC entry 3861 (class 0 OID 16528)
-- Dependencies: 238
-- Data for Name: strapi_core_store_settings; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.strapi_core_store_settings (id, key, value, type, environment, tag) VALUES (2, 'plugin_content_manager_configuration_content_types::admin::role', '{"uid":"admin::role","settings":{"bulkable":true,"filterable":true,"searchable":true,"pageSize":10,"mainField":"name","defaultSortBy":"name","defaultSortOrder":"ASC"},"metadatas":{"id":{"edit":{},"list":{"label":"id","searchable":true,"sortable":true}},"name":{"edit":{"label":"name","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"name","searchable":true,"sortable":true}},"code":{"edit":{"label":"code","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"code","searchable":true,"sortable":true}},"description":{"edit":{"label":"description","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"description","searchable":true,"sortable":true}},"users":{"edit":{"label":"users","description":"","placeholder":"","visible":true,"editable":true,"mainField":"firstname"},"list":{"label":"users","searchable":false,"sortable":false}},"permissions":{"edit":{"label":"permissions","description":"","placeholder":"","visible":true,"editable":true,"mainField":"action"},"list":{"label":"permissions","searchable":false,"sortable":false}},"createdAt":{"edit":{"label":"createdAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"createdAt","searchable":true,"sortable":true}},"updatedAt":{"edit":{"label":"updatedAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"updatedAt","searchable":true,"sortable":true}}},"layouts":{"list":["id","name","code","description"],"edit":[[{"name":"name","size":6},{"name":"code","size":6}],[{"name":"description","size":6},{"name":"users","size":6}],[{"name":"permissions","size":6}]]}}', 'object', NULL, NULL);
INSERT INTO public.strapi_core_store_settings (id, key, value, type, environment, tag) VALUES (3, 'plugin_content_manager_configuration_content_types::admin::user', '{"uid":"admin::user","settings":{"bulkable":true,"filterable":true,"searchable":true,"pageSize":10,"mainField":"firstname","defaultSortBy":"firstname","defaultSortOrder":"ASC"},"metadatas":{"id":{"edit":{},"list":{"label":"id","searchable":true,"sortable":true}},"firstname":{"edit":{"label":"firstname","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"firstname","searchable":true,"sortable":true}},"lastname":{"edit":{"label":"lastname","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"lastname","searchable":true,"sortable":true}},"username":{"edit":{"label":"username","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"username","searchable":true,"sortable":true}},"email":{"edit":{"label":"email","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"email","searchable":true,"sortable":true}},"password":{"edit":{"label":"password","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"password","searchable":true,"sortable":true}},"resetPasswordToken":{"edit":{"label":"resetPasswordToken","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"resetPasswordToken","searchable":true,"sortable":true}},"registrationToken":{"edit":{"label":"registrationToken","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"registrationToken","searchable":true,"sortable":true}},"isActive":{"edit":{"label":"isActive","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"isActive","searchable":true,"sortable":true}},"roles":{"edit":{"label":"roles","description":"","placeholder":"","visible":true,"editable":true,"mainField":"name"},"list":{"label":"roles","searchable":false,"sortable":false}},"blocked":{"edit":{"label":"blocked","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"blocked","searchable":true,"sortable":true}},"preferedLanguage":{"edit":{"label":"preferedLanguage","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"preferedLanguage","searchable":true,"sortable":true}},"createdAt":{"edit":{"label":"createdAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"createdAt","searchable":true,"sortable":true}},"updatedAt":{"edit":{"label":"updatedAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"updatedAt","searchable":true,"sortable":true}}},"layouts":{"list":["id","firstname","lastname","username"],"edit":[[{"name":"firstname","size":6},{"name":"lastname","size":6}],[{"name":"username","size":6},{"name":"email","size":6}],[{"name":"password","size":6},{"name":"resetPasswordToken","size":6}],[{"name":"registrationToken","size":6},{"name":"isActive","size":4}],[{"name":"roles","size":6},{"name":"blocked","size":4}],[{"name":"preferedLanguage","size":6}]]}}', 'object', NULL, NULL);
INSERT INTO public.strapi_core_store_settings (id, key, value, type, environment, tag) VALUES (4, 'plugin_content_manager_configuration_content_types::plugin::users-permissions.permission', '{"uid":"plugin::users-permissions.permission","settings":{"bulkable":true,"filterable":true,"searchable":true,"pageSize":10,"mainField":"action","defaultSortBy":"action","defaultSortOrder":"ASC"},"metadatas":{"id":{"edit":{},"list":{"label":"id","searchable":true,"sortable":true}},"action":{"edit":{"label":"action","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"action","searchable":true,"sortable":true}},"role":{"edit":{"label":"role","description":"","placeholder":"","visible":true,"editable":true,"mainField":"name"},"list":{"label":"role","searchable":true,"sortable":true}},"createdAt":{"edit":{"label":"createdAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"createdAt","searchable":true,"sortable":true}},"updatedAt":{"edit":{"label":"updatedAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"updatedAt","searchable":true,"sortable":true}}},"layouts":{"list":["id","action","role","createdAt"],"edit":[[{"name":"action","size":6},{"name":"role","size":6}]]}}', 'object', NULL, NULL);
INSERT INTO public.strapi_core_store_settings (id, key, value, type, environment, tag) VALUES (5, 'plugin_content_manager_configuration_content_types::admin::permission', '{"uid":"admin::permission","settings":{"bulkable":true,"filterable":true,"searchable":true,"pageSize":10,"mainField":"action","defaultSortBy":"action","defaultSortOrder":"ASC"},"metadatas":{"id":{"edit":{},"list":{"label":"id","searchable":true,"sortable":true}},"action":{"edit":{"label":"action","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"action","searchable":true,"sortable":true}},"subject":{"edit":{"label":"subject","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"subject","searchable":true,"sortable":true}},"properties":{"edit":{"label":"properties","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"properties","searchable":false,"sortable":false}},"conditions":{"edit":{"label":"conditions","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"conditions","searchable":false,"sortable":false}},"role":{"edit":{"label":"role","description":"","placeholder":"","visible":true,"editable":true,"mainField":"name"},"list":{"label":"role","searchable":true,"sortable":true}},"createdAt":{"edit":{"label":"createdAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"createdAt","searchable":true,"sortable":true}},"updatedAt":{"edit":{"label":"updatedAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"updatedAt","searchable":true,"sortable":true}}},"layouts":{"list":["id","action","subject","role"],"edit":[[{"name":"action","size":6},{"name":"subject","size":6}],[{"name":"properties","size":12}],[{"name":"conditions","size":12}],[{"name":"role","size":6}]]}}', 'object', NULL, NULL);
INSERT INTO public.strapi_core_store_settings (id, key, value, type, environment, tag) VALUES (6, 'plugin_content_manager_configuration_content_types::admin::api-token', '{"uid":"admin::api-token","settings":{"bulkable":true,"filterable":true,"searchable":true,"pageSize":10,"mainField":"name","defaultSortBy":"name","defaultSortOrder":"ASC"},"metadatas":{"id":{"edit":{},"list":{"label":"id","searchable":true,"sortable":true}},"name":{"edit":{"label":"name","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"name","searchable":true,"sortable":true}},"description":{"edit":{"label":"description","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"description","searchable":true,"sortable":true}},"type":{"edit":{"label":"type","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"type","searchable":true,"sortable":true}},"accessKey":{"edit":{"label":"accessKey","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"accessKey","searchable":true,"sortable":true}},"lastUsedAt":{"edit":{"label":"lastUsedAt","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"lastUsedAt","searchable":true,"sortable":true}},"permissions":{"edit":{"label":"permissions","description":"","placeholder":"","visible":true,"editable":true,"mainField":"action"},"list":{"label":"permissions","searchable":false,"sortable":false}},"expiresAt":{"edit":{"label":"expiresAt","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"expiresAt","searchable":true,"sortable":true}},"lifespan":{"edit":{"label":"lifespan","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"lifespan","searchable":true,"sortable":true}},"createdAt":{"edit":{"label":"createdAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"createdAt","searchable":true,"sortable":true}},"updatedAt":{"edit":{"label":"updatedAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"updatedAt","searchable":true,"sortable":true}}},"layouts":{"list":["id","name","description","type"],"edit":[[{"name":"name","size":6},{"name":"description","size":6}],[{"name":"type","size":6},{"name":"accessKey","size":6}],[{"name":"lastUsedAt","size":6},{"name":"permissions","size":6}],[{"name":"expiresAt","size":6},{"name":"lifespan","size":4}]]}}', 'object', NULL, NULL);
INSERT INTO public.strapi_core_store_settings (id, key, value, type, environment, tag) VALUES (7, 'plugin_content_manager_configuration_content_types::plugin::i18n.locale', '{"uid":"plugin::i18n.locale","settings":{"bulkable":true,"filterable":true,"searchable":true,"pageSize":10,"mainField":"name","defaultSortBy":"name","defaultSortOrder":"ASC"},"metadatas":{"id":{"edit":{},"list":{"label":"id","searchable":true,"sortable":true}},"name":{"edit":{"label":"name","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"name","searchable":true,"sortable":true}},"code":{"edit":{"label":"code","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"code","searchable":true,"sortable":true}},"createdAt":{"edit":{"label":"createdAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"createdAt","searchable":true,"sortable":true}},"updatedAt":{"edit":{"label":"updatedAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"updatedAt","searchable":true,"sortable":true}}},"layouts":{"list":["id","name","code","createdAt"],"edit":[[{"name":"name","size":6},{"name":"code","size":6}]]}}', 'object', NULL, NULL);
INSERT INTO public.strapi_core_store_settings (id, key, value, type, environment, tag) VALUES (18, 'plugin_i18n_default_locale', '"en"', 'string', NULL, NULL);
INSERT INTO public.strapi_core_store_settings (id, key, value, type, environment, tag) VALUES (19, 'core_admin_auth', '{"providers":{"autoRegister":false,"defaultRole":null}}', 'object', NULL, NULL);
INSERT INTO public.strapi_core_store_settings (id, key, value, type, environment, tag) VALUES (1, 'strapi_content_types_schema', '{"admin::permission":{"collectionName":"admin_permissions","info":{"name":"Permission","description":"","singularName":"permission","pluralName":"permissions","displayName":"Permission"},"options":{},"pluginOptions":{"content-manager":{"visible":false},"content-type-builder":{"visible":false}},"attributes":{"action":{"type":"string","minLength":1,"configurable":false,"required":true},"subject":{"type":"string","minLength":1,"configurable":false,"required":false},"properties":{"type":"json","configurable":false,"required":false,"default":{}},"conditions":{"type":"json","configurable":false,"required":false,"default":[]},"role":{"configurable":false,"type":"relation","relation":"manyToOne","inversedBy":"permissions","target":"admin::role"},"createdAt":{"type":"datetime"},"updatedAt":{"type":"datetime"},"createdBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true},"updatedBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true}},"kind":"collectionType","__schema__":{"collectionName":"admin_permissions","info":{"name":"Permission","description":"","singularName":"permission","pluralName":"permissions","displayName":"Permission"},"options":{},"pluginOptions":{"content-manager":{"visible":false},"content-type-builder":{"visible":false}},"attributes":{"action":{"type":"string","minLength":1,"configurable":false,"required":true},"subject":{"type":"string","minLength":1,"configurable":false,"required":false},"properties":{"type":"json","configurable":false,"required":false,"default":{}},"conditions":{"type":"json","configurable":false,"required":false,"default":[]},"role":{"configurable":false,"type":"relation","relation":"manyToOne","inversedBy":"permissions","target":"admin::role"}},"kind":"collectionType"},"modelType":"contentType","modelName":"permission","connection":"default","uid":"admin::permission","plugin":"admin","globalId":"AdminPermission"},"admin::user":{"collectionName":"admin_users","info":{"name":"User","description":"","singularName":"user","pluralName":"users","displayName":"User"},"pluginOptions":{"content-manager":{"visible":false},"content-type-builder":{"visible":false}},"attributes":{"firstname":{"type":"string","unique":false,"minLength":1,"configurable":false,"required":false},"lastname":{"type":"string","unique":false,"minLength":1,"configurable":false,"required":false},"username":{"type":"string","unique":false,"configurable":false,"required":false},"email":{"type":"email","minLength":6,"configurable":false,"required":true,"unique":true,"private":true},"password":{"type":"password","minLength":6,"configurable":false,"required":false,"private":true},"resetPasswordToken":{"type":"string","configurable":false,"private":true},"registrationToken":{"type":"string","configurable":false,"private":true},"isActive":{"type":"boolean","default":false,"configurable":false,"private":true},"roles":{"configurable":false,"private":true,"type":"relation","relation":"manyToMany","inversedBy":"users","target":"admin::role","collectionName":"strapi_users_roles"},"blocked":{"type":"boolean","default":false,"configurable":false,"private":true},"preferedLanguage":{"type":"string","configurable":false,"required":false},"createdAt":{"type":"datetime"},"updatedAt":{"type":"datetime"},"createdBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true},"updatedBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true}},"kind":"collectionType","__schema__":{"collectionName":"admin_users","info":{"name":"User","description":"","singularName":"user","pluralName":"users","displayName":"User"},"pluginOptions":{"content-manager":{"visible":false},"content-type-builder":{"visible":false}},"attributes":{"firstname":{"type":"string","unique":false,"minLength":1,"configurable":false,"required":false},"lastname":{"type":"string","unique":false,"minLength":1,"configurable":false,"required":false},"username":{"type":"string","unique":false,"configurable":false,"required":false},"email":{"type":"email","minLength":6,"configurable":false,"required":true,"unique":true,"private":true},"password":{"type":"password","minLength":6,"configurable":false,"required":false,"private":true},"resetPasswordToken":{"type":"string","configurable":false,"private":true},"registrationToken":{"type":"string","configurable":false,"private":true},"isActive":{"type":"boolean","default":false,"configurable":false,"private":true},"roles":{"configurable":false,"private":true,"type":"relation","relation":"manyToMany","inversedBy":"users","target":"admin::role","collectionName":"strapi_users_roles"},"blocked":{"type":"boolean","default":false,"configurable":false,"private":true},"preferedLanguage":{"type":"string","configurable":false,"required":false}},"kind":"collectionType"},"modelType":"contentType","modelName":"user","connection":"default","uid":"admin::user","plugin":"admin","globalId":"AdminUser"},"admin::role":{"collectionName":"admin_roles","info":{"name":"Role","description":"","singularName":"role","pluralName":"roles","displayName":"Role"},"options":{},"pluginOptions":{"content-manager":{"visible":false},"content-type-builder":{"visible":false}},"attributes":{"name":{"type":"string","minLength":1,"unique":true,"configurable":false,"required":true},"code":{"type":"string","minLength":1,"unique":true,"configurable":false,"required":true},"description":{"type":"string","configurable":false},"users":{"configurable":false,"type":"relation","relation":"manyToMany","mappedBy":"roles","target":"admin::user"},"permissions":{"configurable":false,"type":"relation","relation":"oneToMany","mappedBy":"role","target":"admin::permission"},"createdAt":{"type":"datetime"},"updatedAt":{"type":"datetime"},"createdBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true},"updatedBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true}},"kind":"collectionType","__schema__":{"collectionName":"admin_roles","info":{"name":"Role","description":"","singularName":"role","pluralName":"roles","displayName":"Role"},"options":{},"pluginOptions":{"content-manager":{"visible":false},"content-type-builder":{"visible":false}},"attributes":{"name":{"type":"string","minLength":1,"unique":true,"configurable":false,"required":true},"code":{"type":"string","minLength":1,"unique":true,"configurable":false,"required":true},"description":{"type":"string","configurable":false},"users":{"configurable":false,"type":"relation","relation":"manyToMany","mappedBy":"roles","target":"admin::user"},"permissions":{"configurable":false,"type":"relation","relation":"oneToMany","mappedBy":"role","target":"admin::permission"}},"kind":"collectionType"},"modelType":"contentType","modelName":"role","connection":"default","uid":"admin::role","plugin":"admin","globalId":"AdminRole"},"admin::api-token":{"collectionName":"strapi_api_tokens","info":{"name":"Api Token","singularName":"api-token","pluralName":"api-tokens","displayName":"Api Token","description":""},"options":{},"pluginOptions":{"content-manager":{"visible":false},"content-type-builder":{"visible":false}},"attributes":{"name":{"type":"string","minLength":1,"configurable":false,"required":true,"unique":true},"description":{"type":"string","minLength":1,"configurable":false,"required":false,"default":""},"type":{"type":"enumeration","enum":["read-only","full-access","custom"],"configurable":false,"required":true,"default":"read-only"},"accessKey":{"type":"string","minLength":1,"configurable":false,"required":true},"lastUsedAt":{"type":"datetime","configurable":false,"required":false},"permissions":{"type":"relation","target":"admin::api-token-permission","relation":"oneToMany","mappedBy":"token","configurable":false,"required":false},"expiresAt":{"type":"datetime","configurable":false,"required":false},"lifespan":{"type":"biginteger","configurable":false,"required":false},"createdAt":{"type":"datetime"},"updatedAt":{"type":"datetime"},"createdBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true},"updatedBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true}},"kind":"collectionType","__schema__":{"collectionName":"strapi_api_tokens","info":{"name":"Api Token","singularName":"api-token","pluralName":"api-tokens","displayName":"Api Token","description":""},"options":{},"pluginOptions":{"content-manager":{"visible":false},"content-type-builder":{"visible":false}},"attributes":{"name":{"type":"string","minLength":1,"configurable":false,"required":true,"unique":true},"description":{"type":"string","minLength":1,"configurable":false,"required":false,"default":""},"type":{"type":"enumeration","enum":["read-only","full-access","custom"],"configurable":false,"required":true,"default":"read-only"},"accessKey":{"type":"string","minLength":1,"configurable":false,"required":true},"lastUsedAt":{"type":"datetime","configurable":false,"required":false},"permissions":{"type":"relation","target":"admin::api-token-permission","relation":"oneToMany","mappedBy":"token","configurable":false,"required":false},"expiresAt":{"type":"datetime","configurable":false,"required":false},"lifespan":{"type":"biginteger","configurable":false,"required":false}},"kind":"collectionType"},"modelType":"contentType","modelName":"api-token","connection":"default","uid":"admin::api-token","plugin":"admin","globalId":"AdminApiToken"},"admin::api-token-permission":{"collectionName":"strapi_api_token_permissions","info":{"name":"API Token Permission","description":"","singularName":"api-token-permission","pluralName":"api-token-permissions","displayName":"API Token Permission"},"options":{},"pluginOptions":{"content-manager":{"visible":false},"content-type-builder":{"visible":false}},"attributes":{"action":{"type":"string","minLength":1,"configurable":false,"required":true},"token":{"configurable":false,"type":"relation","relation":"manyToOne","inversedBy":"permissions","target":"admin::api-token"},"createdAt":{"type":"datetime"},"updatedAt":{"type":"datetime"},"createdBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true},"updatedBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true}},"kind":"collectionType","__schema__":{"collectionName":"strapi_api_token_permissions","info":{"name":"API Token Permission","description":"","singularName":"api-token-permission","pluralName":"api-token-permissions","displayName":"API Token Permission"},"options":{},"pluginOptions":{"content-manager":{"visible":false},"content-type-builder":{"visible":false}},"attributes":{"action":{"type":"string","minLength":1,"configurable":false,"required":true},"token":{"configurable":false,"type":"relation","relation":"manyToOne","inversedBy":"permissions","target":"admin::api-token"}},"kind":"collectionType"},"modelType":"contentType","modelName":"api-token-permission","connection":"default","uid":"admin::api-token-permission","plugin":"admin","globalId":"AdminApiTokenPermission"},"plugin::upload.file":{"collectionName":"files","info":{"singularName":"file","pluralName":"files","displayName":"File","description":""},"options":{},"pluginOptions":{"content-manager":{"visible":false},"content-type-builder":{"visible":false}},"attributes":{"name":{"type":"string","configurable":false,"required":true},"alternativeText":{"type":"string","configurable":false},"caption":{"type":"string","configurable":false},"width":{"type":"integer","configurable":false},"height":{"type":"integer","configurable":false},"formats":{"type":"json","configurable":false},"hash":{"type":"string","configurable":false,"required":true},"ext":{"type":"string","configurable":false},"mime":{"type":"string","configurable":false,"required":true},"size":{"type":"decimal","configurable":false,"required":true},"url":{"type":"string","configurable":false,"required":true},"previewUrl":{"type":"string","configurable":false},"provider":{"type":"string","configurable":false,"required":true},"provider_metadata":{"type":"json","configurable":false},"related":{"type":"relation","relation":"morphToMany","configurable":false},"folder":{"type":"relation","relation":"manyToOne","target":"plugin::upload.folder","inversedBy":"files","private":true},"folderPath":{"type":"string","min":1,"required":true,"private":true},"createdAt":{"type":"datetime"},"updatedAt":{"type":"datetime"},"createdBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true},"updatedBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true}},"indexes":[{"name":"upload_files_folder_path_index","columns":["folder_path"],"type":null}],"kind":"collectionType","__schema__":{"collectionName":"files","info":{"singularName":"file","pluralName":"files","displayName":"File","description":""},"options":{},"pluginOptions":{"content-manager":{"visible":false},"content-type-builder":{"visible":false}},"attributes":{"name":{"type":"string","configurable":false,"required":true},"alternativeText":{"type":"string","configurable":false},"caption":{"type":"string","configurable":false},"width":{"type":"integer","configurable":false},"height":{"type":"integer","configurable":false},"formats":{"type":"json","configurable":false},"hash":{"type":"string","configurable":false,"required":true},"ext":{"type":"string","configurable":false},"mime":{"type":"string","configurable":false,"required":true},"size":{"type":"decimal","configurable":false,"required":true},"url":{"type":"string","configurable":false,"required":true},"previewUrl":{"type":"string","configurable":false},"provider":{"type":"string","configurable":false,"required":true},"provider_metadata":{"type":"json","configurable":false},"related":{"type":"relation","relation":"morphToMany","configurable":false},"folder":{"type":"relation","relation":"manyToOne","target":"plugin::upload.folder","inversedBy":"files","private":true},"folderPath":{"type":"string","min":1,"required":true,"private":true}},"kind":"collectionType"},"modelType":"contentType","modelName":"file","connection":"default","uid":"plugin::upload.file","plugin":"upload","globalId":"UploadFile"},"plugin::upload.folder":{"collectionName":"upload_folders","info":{"singularName":"folder","pluralName":"folders","displayName":"Folder"},"options":{},"pluginOptions":{"content-manager":{"visible":false},"content-type-builder":{"visible":false}},"attributes":{"name":{"type":"string","min":1,"required":true},"pathId":{"type":"integer","unique":true,"required":true},"parent":{"type":"relation","relation":"manyToOne","target":"plugin::upload.folder","inversedBy":"children"},"children":{"type":"relation","relation":"oneToMany","target":"plugin::upload.folder","mappedBy":"parent"},"files":{"type":"relation","relation":"oneToMany","target":"plugin::upload.file","mappedBy":"folder"},"path":{"type":"string","min":1,"required":true},"createdAt":{"type":"datetime"},"updatedAt":{"type":"datetime"},"createdBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true},"updatedBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true}},"indexes":[{"name":"upload_folders_path_id_index","columns":["path_id"],"type":"unique"},{"name":"upload_folders_path_index","columns":["path"],"type":"unique"}],"kind":"collectionType","__schema__":{"collectionName":"upload_folders","info":{"singularName":"folder","pluralName":"folders","displayName":"Folder"},"options":{},"pluginOptions":{"content-manager":{"visible":false},"content-type-builder":{"visible":false}},"attributes":{"name":{"type":"string","min":1,"required":true},"pathId":{"type":"integer","unique":true,"required":true},"parent":{"type":"relation","relation":"manyToOne","target":"plugin::upload.folder","inversedBy":"children"},"children":{"type":"relation","relation":"oneToMany","target":"plugin::upload.folder","mappedBy":"parent"},"files":{"type":"relation","relation":"oneToMany","target":"plugin::upload.file","mappedBy":"folder"},"path":{"type":"string","min":1,"required":true}},"kind":"collectionType"},"modelType":"contentType","modelName":"folder","connection":"default","uid":"plugin::upload.folder","plugin":"upload","globalId":"UploadFolder"},"plugin::i18n.locale":{"info":{"singularName":"locale","pluralName":"locales","collectionName":"locales","displayName":"Locale","description":""},"options":{"draftAndPublish":false},"pluginOptions":{"content-manager":{"visible":false},"content-type-builder":{"visible":false}},"attributes":{"name":{"type":"string","min":1,"max":50,"configurable":false},"code":{"type":"string","unique":true,"configurable":false},"createdAt":{"type":"datetime"},"updatedAt":{"type":"datetime"},"createdBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true},"updatedBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true}},"kind":"collectionType","__schema__":{"info":{"singularName":"locale","pluralName":"locales","collectionName":"locales","displayName":"Locale","description":""},"options":{"draftAndPublish":false},"pluginOptions":{"content-manager":{"visible":false},"content-type-builder":{"visible":false}},"attributes":{"name":{"type":"string","min":1,"max":50,"configurable":false},"code":{"type":"string","unique":true,"configurable":false}},"kind":"collectionType"},"modelType":"contentType","modelName":"locale","connection":"default","uid":"plugin::i18n.locale","plugin":"i18n","collectionName":"i18n_locale","globalId":"I18NLocale"},"plugin::users-permissions.permission":{"collectionName":"up_permissions","info":{"name":"permission","description":"","singularName":"permission","pluralName":"permissions","displayName":"Permission"},"pluginOptions":{"content-manager":{"visible":false},"content-type-builder":{"visible":false}},"attributes":{"action":{"type":"string","required":true,"configurable":false},"role":{"type":"relation","relation":"manyToOne","target":"plugin::users-permissions.role","inversedBy":"permissions","configurable":false},"createdAt":{"type":"datetime"},"updatedAt":{"type":"datetime"},"createdBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true},"updatedBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true}},"kind":"collectionType","__schema__":{"collectionName":"up_permissions","info":{"name":"permission","description":"","singularName":"permission","pluralName":"permissions","displayName":"Permission"},"pluginOptions":{"content-manager":{"visible":false},"content-type-builder":{"visible":false}},"attributes":{"action":{"type":"string","required":true,"configurable":false},"role":{"type":"relation","relation":"manyToOne","target":"plugin::users-permissions.role","inversedBy":"permissions","configurable":false}},"kind":"collectionType"},"modelType":"contentType","modelName":"permission","connection":"default","uid":"plugin::users-permissions.permission","plugin":"users-permissions","globalId":"UsersPermissionsPermission"},"plugin::users-permissions.role":{"collectionName":"up_roles","info":{"name":"role","description":"","singularName":"role","pluralName":"roles","displayName":"Role"},"pluginOptions":{"content-manager":{"visible":false},"content-type-builder":{"visible":false}},"attributes":{"name":{"type":"string","minLength":3,"required":true,"configurable":false},"description":{"type":"string","configurable":false},"type":{"type":"string","unique":true,"configurable":false},"permissions":{"type":"relation","relation":"oneToMany","target":"plugin::users-permissions.permission","mappedBy":"role","configurable":false},"users":{"type":"relation","relation":"oneToMany","target":"plugin::users-permissions.user","mappedBy":"role","configurable":false},"createdAt":{"type":"datetime"},"updatedAt":{"type":"datetime"},"createdBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true},"updatedBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true}},"kind":"collectionType","__schema__":{"collectionName":"up_roles","info":{"name":"role","description":"","singularName":"role","pluralName":"roles","displayName":"Role"},"pluginOptions":{"content-manager":{"visible":false},"content-type-builder":{"visible":false}},"attributes":{"name":{"type":"string","minLength":3,"required":true,"configurable":false},"description":{"type":"string","configurable":false},"type":{"type":"string","unique":true,"configurable":false},"permissions":{"type":"relation","relation":"oneToMany","target":"plugin::users-permissions.permission","mappedBy":"role","configurable":false},"users":{"type":"relation","relation":"oneToMany","target":"plugin::users-permissions.user","mappedBy":"role","configurable":false}},"kind":"collectionType"},"modelType":"contentType","modelName":"role","connection":"default","uid":"plugin::users-permissions.role","plugin":"users-permissions","globalId":"UsersPermissionsRole"},"plugin::users-permissions.user":{"collectionName":"up_users","info":{"name":"user","description":"","singularName":"user","pluralName":"users","displayName":"User"},"options":{"draftAndPublish":false,"timestamps":true},"attributes":{"username":{"type":"string","minLength":3,"unique":true,"configurable":false,"required":true},"email":{"type":"email","minLength":6,"configurable":false,"required":true},"provider":{"type":"string","configurable":false},"password":{"type":"password","minLength":6,"configurable":false,"private":true},"resetPasswordToken":{"type":"string","configurable":false,"private":true},"confirmationToken":{"type":"string","configurable":false,"private":true},"confirmed":{"type":"boolean","default":false,"configurable":false},"blocked":{"type":"boolean","default":false,"configurable":false},"role":{"type":"relation","relation":"manyToOne","target":"plugin::users-permissions.role","inversedBy":"users","configurable":false},"createdAt":{"type":"datetime"},"updatedAt":{"type":"datetime"},"createdBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true},"updatedBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true}},"config":{"attributes":{"resetPasswordToken":{"hidden":true},"confirmationToken":{"hidden":true},"provider":{"hidden":true}}},"kind":"collectionType","__schema__":{"collectionName":"up_users","info":{"name":"user","description":"","singularName":"user","pluralName":"users","displayName":"User"},"options":{"draftAndPublish":false,"timestamps":true},"attributes":{"username":{"type":"string","minLength":3,"unique":true,"configurable":false,"required":true},"email":{"type":"email","minLength":6,"configurable":false,"required":true},"provider":{"type":"string","configurable":false},"password":{"type":"password","minLength":6,"configurable":false,"private":true},"resetPasswordToken":{"type":"string","configurable":false,"private":true},"confirmationToken":{"type":"string","configurable":false,"private":true},"confirmed":{"type":"boolean","default":false,"configurable":false},"blocked":{"type":"boolean","default":false,"configurable":false},"role":{"type":"relation","relation":"manyToOne","target":"plugin::users-permissions.role","inversedBy":"users","configurable":false}},"kind":"collectionType"},"modelType":"contentType","modelName":"user","connection":"default","uid":"plugin::users-permissions.user","plugin":"users-permissions","globalId":"UsersPermissionsUser"},"api::available-products-page.available-products-page":{"kind":"singleType","collectionName":"available_products_pages","info":{"singularName":"available-products-page","pluralName":"available-products-pages","displayName":"Available Products Page","description":""},"options":{"draftAndPublish":true},"pluginOptions":{"i18n":{"localized":true}},"attributes":{"seo":{"type":"component","repeatable":false,"pluginOptions":{"i18n":{"localized":true}},"component":"shared.seo"},"createdAt":{"type":"datetime"},"updatedAt":{"type":"datetime"},"publishedAt":{"type":"datetime","configurable":false,"writable":true,"visible":false},"createdBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true},"updatedBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true},"localizations":{"writable":true,"private":false,"configurable":false,"visible":false,"type":"relation","relation":"oneToMany","target":"api::available-products-page.available-products-page"},"locale":{"writable":true,"private":false,"configurable":false,"visible":false,"type":"string"}},"__schema__":{"collectionName":"available_products_pages","info":{"singularName":"available-products-page","pluralName":"available-products-pages","displayName":"Available Products Page","description":""},"options":{"draftAndPublish":true},"pluginOptions":{"i18n":{"localized":true}},"attributes":{"seo":{"type":"component","repeatable":false,"pluginOptions":{"i18n":{"localized":true}},"component":"shared.seo"}},"kind":"singleType"},"modelType":"contentType","modelName":"available-products-page","connection":"default","uid":"api::available-products-page.available-products-page","apiName":"available-products-page","globalId":"AvailableProductsPage","actions":{},"lifecycles":{}},"api::basket-page.basket-page":{"kind":"singleType","collectionName":"basket_pages","info":{"singularName":"basket-page","pluralName":"basket-pages","displayName":"Basket Page"},"options":{"draftAndPublish":true},"pluginOptions":{"i18n":{"localized":true}},"attributes":{"seo":{"type":"component","repeatable":false,"pluginOptions":{"i18n":{"localized":true}},"component":"shared.seo"},"createdAt":{"type":"datetime"},"updatedAt":{"type":"datetime"},"publishedAt":{"type":"datetime","configurable":false,"writable":true,"visible":false},"createdBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true},"updatedBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true},"localizations":{"writable":true,"private":false,"configurable":false,"visible":false,"type":"relation","relation":"oneToMany","target":"api::basket-page.basket-page"},"locale":{"writable":true,"private":false,"configurable":false,"visible":false,"type":"string"}},"__schema__":{"collectionName":"basket_pages","info":{"singularName":"basket-page","pluralName":"basket-pages","displayName":"Basket Page"},"options":{"draftAndPublish":true},"pluginOptions":{"i18n":{"localized":true}},"attributes":{"seo":{"type":"component","repeatable":false,"pluginOptions":{"i18n":{"localized":true}},"component":"shared.seo"}},"kind":"singleType"},"modelType":"contentType","modelName":"basket-page","connection":"default","uid":"api::basket-page.basket-page","apiName":"basket-page","globalId":"BasketPage","actions":{},"lifecycles":{}},"api::dashboard-page.dashboard-page":{"kind":"singleType","collectionName":"dashboard_pages","info":{"singularName":"dashboard-page","pluralName":"dashboard-pages","displayName":"Dashboard Page"},"options":{"draftAndPublish":true},"pluginOptions":{"i18n":{"localized":true}},"attributes":{"seo":{"type":"component","repeatable":false,"pluginOptions":{"i18n":{"localized":true}},"component":"shared.seo"},"createdAt":{"type":"datetime"},"updatedAt":{"type":"datetime"},"publishedAt":{"type":"datetime","configurable":false,"writable":true,"visible":false},"createdBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true},"updatedBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true},"localizations":{"writable":true,"private":false,"configurable":false,"visible":false,"type":"relation","relation":"oneToMany","target":"api::dashboard-page.dashboard-page"},"locale":{"writable":true,"private":false,"configurable":false,"visible":false,"type":"string"}},"__schema__":{"collectionName":"dashboard_pages","info":{"singularName":"dashboard-page","pluralName":"dashboard-pages","displayName":"Dashboard Page"},"options":{"draftAndPublish":true},"pluginOptions":{"i18n":{"localized":true}},"attributes":{"seo":{"type":"component","repeatable":false,"pluginOptions":{"i18n":{"localized":true}},"component":"shared.seo"}},"kind":"singleType"},"modelType":"contentType","modelName":"dashboard-page","connection":"default","uid":"api::dashboard-page.dashboard-page","apiName":"dashboard-page","globalId":"DashboardPage","actions":{},"lifecycles":{}},"api::download-center-page.download-center-page":{"kind":"singleType","collectionName":"download_center_pages","info":{"singularName":"download-center-page","pluralName":"download-center-pages","displayName":"Download Center Page","description":""},"options":{"draftAndPublish":true},"pluginOptions":{"i18n":{"localized":true}},"attributes":{"seo":{"type":"component","repeatable":false,"component":"shared.seo","pluginOptions":{"i18n":{"localized":true}}},"createdAt":{"type":"datetime"},"updatedAt":{"type":"datetime"},"publishedAt":{"type":"datetime","configurable":false,"writable":true,"visible":false},"createdBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true},"updatedBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true},"localizations":{"writable":true,"private":false,"configurable":false,"visible":false,"type":"relation","relation":"oneToMany","target":"api::download-center-page.download-center-page"},"locale":{"writable":true,"private":false,"configurable":false,"visible":false,"type":"string"}},"__schema__":{"collectionName":"download_center_pages","info":{"singularName":"download-center-page","pluralName":"download-center-pages","displayName":"Download Center Page","description":""},"options":{"draftAndPublish":true},"pluginOptions":{"i18n":{"localized":true}},"attributes":{"seo":{"type":"component","repeatable":false,"component":"shared.seo","pluginOptions":{"i18n":{"localized":true}}}},"kind":"singleType"},"modelType":"contentType","modelName":"download-center-page","connection":"default","uid":"api::download-center-page.download-center-page","apiName":"download-center-page","globalId":"DownloadCenterPage","actions":{},"lifecycles":{}},"api::home-page.home-page":{"kind":"singleType","collectionName":"home_pages","info":{"singularName":"home-page","pluralName":"home-pages","displayName":"Home Page"},"options":{"draftAndPublish":true},"pluginOptions":{"i18n":{"localized":true}},"attributes":{"seo":{"type":"component","repeatable":false,"pluginOptions":{"i18n":{"localized":true}},"component":"shared.seo"},"heroSlider":{"type":"component","repeatable":false,"pluginOptions":{"i18n":{"localized":true}},"component":"blocks.hero-slider"},"createdAt":{"type":"datetime"},"updatedAt":{"type":"datetime"},"publishedAt":{"type":"datetime","configurable":false,"writable":true,"visible":false},"createdBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true},"updatedBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true},"localizations":{"writable":true,"private":false,"configurable":false,"visible":false,"type":"relation","relation":"oneToMany","target":"api::home-page.home-page"},"locale":{"writable":true,"private":false,"configurable":false,"visible":false,"type":"string"}},"__schema__":{"collectionName":"home_pages","info":{"singularName":"home-page","pluralName":"home-pages","displayName":"Home Page"},"options":{"draftAndPublish":true},"pluginOptions":{"i18n":{"localized":true}},"attributes":{"seo":{"type":"component","repeatable":false,"pluginOptions":{"i18n":{"localized":true}},"component":"shared.seo"},"heroSlider":{"type":"component","repeatable":false,"pluginOptions":{"i18n":{"localized":true}},"component":"blocks.hero-slider"}},"kind":"singleType"},"modelType":"contentType","modelName":"home-page","connection":"default","uid":"api::home-page.home-page","apiName":"home-page","globalId":"HomePage","actions":{},"lifecycles":{}},"api::news-page.news-page":{"kind":"singleType","collectionName":"news_pages","info":{"singularName":"news-page","pluralName":"news-pages","displayName":"News Page","description":""},"options":{"draftAndPublish":true},"pluginOptions":{"i18n":{"localized":true}},"attributes":{"seo":{"type":"component","repeatable":false,"component":"shared.seo","pluginOptions":{"i18n":{"localized":true}}},"createdAt":{"type":"datetime"},"updatedAt":{"type":"datetime"},"publishedAt":{"type":"datetime","configurable":false,"writable":true,"visible":false},"createdBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true},"updatedBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true},"localizations":{"writable":true,"private":false,"configurable":false,"visible":false,"type":"relation","relation":"oneToMany","target":"api::news-page.news-page"},"locale":{"writable":true,"private":false,"configurable":false,"visible":false,"type":"string"}},"__schema__":{"collectionName":"news_pages","info":{"singularName":"news-page","pluralName":"news-pages","displayName":"News Page","description":""},"options":{"draftAndPublish":true},"pluginOptions":{"i18n":{"localized":true}},"attributes":{"seo":{"type":"component","repeatable":false,"component":"shared.seo","pluginOptions":{"i18n":{"localized":true}}}},"kind":"singleType"},"modelType":"contentType","modelName":"news-page","connection":"default","uid":"api::news-page.news-page","apiName":"news-page","globalId":"NewsPage","actions":{},"lifecycles":{}},"api::order-item-page.order-item-page":{"kind":"singleType","collectionName":"order_item_pages","info":{"singularName":"order-item-page","pluralName":"order-item-pages","displayName":"Order Item Page","description":""},"options":{"draftAndPublish":true},"pluginOptions":{"i18n":{"localized":true}},"attributes":{"seo":{"type":"component","repeatable":false,"pluginOptions":{"i18n":{"localized":true}},"component":"shared.seo"},"createdAt":{"type":"datetime"},"updatedAt":{"type":"datetime"},"publishedAt":{"type":"datetime","configurable":false,"writable":true,"visible":false},"createdBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true},"updatedBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true},"localizations":{"writable":true,"private":false,"configurable":false,"visible":false,"type":"relation","relation":"oneToMany","target":"api::order-item-page.order-item-page"},"locale":{"writable":true,"private":false,"configurable":false,"visible":false,"type":"string"}},"__schema__":{"collectionName":"order_item_pages","info":{"singularName":"order-item-page","pluralName":"order-item-pages","displayName":"Order Item Page","description":""},"options":{"draftAndPublish":true},"pluginOptions":{"i18n":{"localized":true}},"attributes":{"seo":{"type":"component","repeatable":false,"pluginOptions":{"i18n":{"localized":true}},"component":"shared.seo"}},"kind":"singleType"},"modelType":"contentType","modelName":"order-item-page","connection":"default","uid":"api::order-item-page.order-item-page","apiName":"order-item-page","globalId":"OrderItemPage","actions":{},"lifecycles":{}},"api::order-page.order-page":{"kind":"singleType","collectionName":"order_pages","info":{"singularName":"order-page","pluralName":"order-pages","displayName":"Order Page"},"options":{"draftAndPublish":true},"pluginOptions":{"i18n":{"localized":true}},"attributes":{"seo":{"type":"component","repeatable":false,"pluginOptions":{"i18n":{"localized":true}},"component":"shared.seo"},"createdAt":{"type":"datetime"},"updatedAt":{"type":"datetime"},"publishedAt":{"type":"datetime","configurable":false,"writable":true,"visible":false},"createdBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true},"updatedBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true},"localizations":{"writable":true,"private":false,"configurable":false,"visible":false,"type":"relation","relation":"oneToMany","target":"api::order-page.order-page"},"locale":{"writable":true,"private":false,"configurable":false,"visible":false,"type":"string"}},"__schema__":{"collectionName":"order_pages","info":{"singularName":"order-page","pluralName":"order-pages","displayName":"Order Page"},"options":{"draftAndPublish":true},"pluginOptions":{"i18n":{"localized":true}},"attributes":{"seo":{"type":"component","repeatable":false,"pluginOptions":{"i18n":{"localized":true}},"component":"shared.seo"}},"kind":"singleType"},"modelType":"contentType","modelName":"order-page","connection":"default","uid":"api::order-page.order-page","apiName":"order-page","globalId":"OrderPage","actions":{},"lifecycles":{}},"api::orders-page.orders-page":{"kind":"singleType","collectionName":"orders_pages","info":{"singularName":"orders-page","pluralName":"orders-pages","displayName":"Orders Page","description":""},"options":{"draftAndPublish":true},"pluginOptions":{"i18n":{"localized":true}},"attributes":{"seo":{"type":"component","repeatable":false,"component":"shared.seo","pluginOptions":{"i18n":{"localized":true}}},"createdAt":{"type":"datetime"},"updatedAt":{"type":"datetime"},"publishedAt":{"type":"datetime","configurable":false,"writable":true,"visible":false},"createdBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true},"updatedBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true},"localizations":{"writable":true,"private":false,"configurable":false,"visible":false,"type":"relation","relation":"oneToMany","target":"api::orders-page.orders-page"},"locale":{"writable":true,"private":false,"configurable":false,"visible":false,"type":"string"}},"__schema__":{"collectionName":"orders_pages","info":{"singularName":"orders-page","pluralName":"orders-pages","displayName":"Orders Page","description":""},"options":{"draftAndPublish":true},"pluginOptions":{"i18n":{"localized":true}},"attributes":{"seo":{"type":"component","repeatable":false,"component":"shared.seo","pluginOptions":{"i18n":{"localized":true}}}},"kind":"singleType"},"modelType":"contentType","modelName":"orders-page","connection":"default","uid":"api::orders-page.orders-page","apiName":"orders-page","globalId":"OrdersPage","actions":{},"lifecycles":{}},"api::outlet-page.outlet-page":{"kind":"singleType","collectionName":"outlet_pages","info":{"singularName":"outlet-page","pluralName":"outlet-pages","displayName":"Outlet Page","description":""},"options":{"draftAndPublish":true},"pluginOptions":{"i18n":{"localized":true}},"attributes":{"seo":{"type":"component","repeatable":false,"component":"shared.seo","pluginOptions":{"i18n":{"localized":true}}},"createdAt":{"type":"datetime"},"updatedAt":{"type":"datetime"},"publishedAt":{"type":"datetime","configurable":false,"writable":true,"visible":false},"createdBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true},"updatedBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true},"localizations":{"writable":true,"private":false,"configurable":false,"visible":false,"type":"relation","relation":"oneToMany","target":"api::outlet-page.outlet-page"},"locale":{"writable":true,"private":false,"configurable":false,"visible":false,"type":"string"}},"__schema__":{"collectionName":"outlet_pages","info":{"singularName":"outlet-page","pluralName":"outlet-pages","displayName":"Outlet Page","description":""},"options":{"draftAndPublish":true},"pluginOptions":{"i18n":{"localized":true}},"attributes":{"seo":{"type":"component","repeatable":false,"component":"shared.seo","pluginOptions":{"i18n":{"localized":true}}}},"kind":"singleType"},"modelType":"contentType","modelName":"outlet-page","connection":"default","uid":"api::outlet-page.outlet-page","apiName":"outlet-page","globalId":"OutletPage","actions":{},"lifecycles":{}},"api::privacy-policy-page.privacy-policy-page":{"kind":"singleType","collectionName":"privacy_policy_pages","info":{"singularName":"privacy-policy-page","pluralName":"privacy-policy-pages","displayName":"Privacy Policy Page","description":""},"options":{"draftAndPublish":true},"pluginOptions":{"i18n":{"localized":true}},"attributes":{"seo":{"type":"component","repeatable":false,"pluginOptions":{"i18n":{"localized":true}},"component":"shared.seo"},"Text":{"pluginOptions":{"i18n":{"localized":true}},"type":"richtext"},"createdAt":{"type":"datetime"},"updatedAt":{"type":"datetime"},"publishedAt":{"type":"datetime","configurable":false,"writable":true,"visible":false},"createdBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true},"updatedBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true},"localizations":{"writable":true,"private":false,"configurable":false,"visible":false,"type":"relation","relation":"oneToMany","target":"api::privacy-policy-page.privacy-policy-page"},"locale":{"writable":true,"private":false,"configurable":false,"visible":false,"type":"string"}},"__schema__":{"collectionName":"privacy_policy_pages","info":{"singularName":"privacy-policy-page","pluralName":"privacy-policy-pages","displayName":"Privacy Policy Page","description":""},"options":{"draftAndPublish":true},"pluginOptions":{"i18n":{"localized":true}},"attributes":{"seo":{"type":"component","repeatable":false,"pluginOptions":{"i18n":{"localized":true}},"component":"shared.seo"},"Text":{"pluginOptions":{"i18n":{"localized":true}},"type":"richtext"}},"kind":"singleType"},"modelType":"contentType","modelName":"privacy-policy-page","connection":"default","uid":"api::privacy-policy-page.privacy-policy-page","apiName":"privacy-policy-page","globalId":"PrivacyPolicyPage","actions":{},"lifecycles":{}},"api::regulations-page.regulations-page":{"kind":"singleType","collectionName":"regulations_pages","info":{"singularName":"regulations-page","pluralName":"regulations-pages","displayName":"Regulations Page","description":""},"options":{"draftAndPublish":true},"pluginOptions":{"i18n":{"localized":true}},"attributes":{"seo":{"type":"component","repeatable":false,"pluginOptions":{"i18n":{"localized":true}},"component":"shared.seo"},"Text":{"pluginOptions":{"i18n":{"localized":true}},"type":"richtext"},"createdAt":{"type":"datetime"},"updatedAt":{"type":"datetime"},"publishedAt":{"type":"datetime","configurable":false,"writable":true,"visible":false},"createdBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true},"updatedBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true},"localizations":{"writable":true,"private":false,"configurable":false,"visible":false,"type":"relation","relation":"oneToMany","target":"api::regulations-page.regulations-page"},"locale":{"writable":true,"private":false,"configurable":false,"visible":false,"type":"string"}},"__schema__":{"collectionName":"regulations_pages","info":{"singularName":"regulations-page","pluralName":"regulations-pages","displayName":"Regulations Page","description":""},"options":{"draftAndPublish":true},"pluginOptions":{"i18n":{"localized":true}},"attributes":{"seo":{"type":"component","repeatable":false,"pluginOptions":{"i18n":{"localized":true}},"component":"shared.seo"},"Text":{"pluginOptions":{"i18n":{"localized":true}},"type":"richtext"}},"kind":"singleType"},"modelType":"contentType","modelName":"regulations-page","connection":"default","uid":"api::regulations-page.regulations-page","apiName":"regulations-page","globalId":"RegulationsPage","actions":{},"lifecycles":{}},"api::search-products-page.search-products-page":{"kind":"singleType","collectionName":"search_products_pages","info":{"singularName":"search-products-page","pluralName":"search-products-pages","displayName":"Search Products Page"},"options":{"draftAndPublish":true},"pluginOptions":{"i18n":{"localized":true}},"attributes":{"seo":{"type":"component","repeatable":false,"pluginOptions":{"i18n":{"localized":true}},"component":"shared.seo"},"createdAt":{"type":"datetime"},"updatedAt":{"type":"datetime"},"publishedAt":{"type":"datetime","configurable":false,"writable":true,"visible":false},"createdBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true},"updatedBy":{"type":"relation","relation":"oneToOne","target":"admin::user","configurable":false,"writable":false,"visible":false,"useJoinTable":false,"private":true},"localizations":{"writable":true,"private":false,"configurable":false,"visible":false,"type":"relation","relation":"oneToMany","target":"api::search-products-page.search-products-page"},"locale":{"writable":true,"private":false,"configurable":false,"visible":false,"type":"string"}},"__schema__":{"collectionName":"search_products_pages","info":{"singularName":"search-products-page","pluralName":"search-products-pages","displayName":"Search Products Page"},"options":{"draftAndPublish":true},"pluginOptions":{"i18n":{"localized":true}},"attributes":{"seo":{"type":"component","repeatable":false,"pluginOptions":{"i18n":{"localized":true}},"component":"shared.seo"}},"kind":"singleType"},"modelType":"contentType","modelName":"search-products-page","connection":"default","uid":"api::search-products-page.search-products-page","apiName":"search-products-page","globalId":"SearchProductsPage","actions":{},"lifecycles":{}}}', 'object', NULL, NULL);
INSERT INTO public.strapi_core_store_settings (id, key, value, type, environment, tag) VALUES (8, 'plugin_content_manager_configuration_content_types::plugin::upload.folder', '{"uid":"plugin::upload.folder","settings":{"bulkable":true,"filterable":true,"searchable":true,"pageSize":10,"mainField":"name","defaultSortBy":"name","defaultSortOrder":"ASC"},"metadatas":{"id":{"edit":{},"list":{"label":"id","searchable":true,"sortable":true}},"name":{"edit":{"label":"name","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"name","searchable":true,"sortable":true}},"pathId":{"edit":{"label":"pathId","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"pathId","searchable":true,"sortable":true}},"parent":{"edit":{"label":"parent","description":"","placeholder":"","visible":true,"editable":true,"mainField":"name"},"list":{"label":"parent","searchable":true,"sortable":true}},"children":{"edit":{"label":"children","description":"","placeholder":"","visible":true,"editable":true,"mainField":"name"},"list":{"label":"children","searchable":false,"sortable":false}},"files":{"edit":{"label":"files","description":"","placeholder":"","visible":true,"editable":true,"mainField":"name"},"list":{"label":"files","searchable":false,"sortable":false}},"path":{"edit":{"label":"path","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"path","searchable":true,"sortable":true}},"createdAt":{"edit":{"label":"createdAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"createdAt","searchable":true,"sortable":true}},"updatedAt":{"edit":{"label":"updatedAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"updatedAt","searchable":true,"sortable":true}}},"layouts":{"list":["id","name","pathId","parent"],"edit":[[{"name":"name","size":6},{"name":"pathId","size":4}],[{"name":"parent","size":6},{"name":"children","size":6}],[{"name":"files","size":6},{"name":"path","size":6}]]}}', 'object', NULL, NULL);
INSERT INTO public.strapi_core_store_settings (id, key, value, type, environment, tag) VALUES (10, 'plugin_content_manager_configuration_content_types::admin::api-token-permission', '{"uid":"admin::api-token-permission","settings":{"bulkable":true,"filterable":true,"searchable":true,"pageSize":10,"mainField":"action","defaultSortBy":"action","defaultSortOrder":"ASC"},"metadatas":{"id":{"edit":{},"list":{"label":"id","searchable":true,"sortable":true}},"action":{"edit":{"label":"action","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"action","searchable":true,"sortable":true}},"token":{"edit":{"label":"token","description":"","placeholder":"","visible":true,"editable":true,"mainField":"name"},"list":{"label":"token","searchable":true,"sortable":true}},"createdAt":{"edit":{"label":"createdAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"createdAt","searchable":true,"sortable":true}},"updatedAt":{"edit":{"label":"updatedAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"updatedAt","searchable":true,"sortable":true}}},"layouts":{"list":["id","action","token","createdAt"],"edit":[[{"name":"action","size":6},{"name":"token","size":6}]]}}', 'object', NULL, NULL);
INSERT INTO public.strapi_core_store_settings (id, key, value, type, environment, tag) VALUES (9, 'plugin_content_manager_configuration_content_types::plugin::upload.file', '{"uid":"plugin::upload.file","settings":{"bulkable":true,"filterable":true,"searchable":true,"pageSize":10,"mainField":"name","defaultSortBy":"name","defaultSortOrder":"ASC"},"metadatas":{"id":{"edit":{},"list":{"label":"id","searchable":true,"sortable":true}},"name":{"edit":{"label":"name","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"name","searchable":true,"sortable":true}},"alternativeText":{"edit":{"label":"alternativeText","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"alternativeText","searchable":true,"sortable":true}},"caption":{"edit":{"label":"caption","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"caption","searchable":true,"sortable":true}},"width":{"edit":{"label":"width","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"width","searchable":true,"sortable":true}},"height":{"edit":{"label":"height","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"height","searchable":true,"sortable":true}},"formats":{"edit":{"label":"formats","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"formats","searchable":false,"sortable":false}},"hash":{"edit":{"label":"hash","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"hash","searchable":true,"sortable":true}},"ext":{"edit":{"label":"ext","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"ext","searchable":true,"sortable":true}},"mime":{"edit":{"label":"mime","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"mime","searchable":true,"sortable":true}},"size":{"edit":{"label":"size","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"size","searchable":true,"sortable":true}},"url":{"edit":{"label":"url","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"url","searchable":true,"sortable":true}},"previewUrl":{"edit":{"label":"previewUrl","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"previewUrl","searchable":true,"sortable":true}},"provider":{"edit":{"label":"provider","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"provider","searchable":true,"sortable":true}},"provider_metadata":{"edit":{"label":"provider_metadata","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"provider_metadata","searchable":false,"sortable":false}},"folder":{"edit":{"label":"folder","description":"","placeholder":"","visible":true,"editable":true,"mainField":"name"},"list":{"label":"folder","searchable":true,"sortable":true}},"folderPath":{"edit":{"label":"folderPath","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"folderPath","searchable":true,"sortable":true}},"createdAt":{"edit":{"label":"createdAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"createdAt","searchable":true,"sortable":true}},"updatedAt":{"edit":{"label":"updatedAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"updatedAt","searchable":true,"sortable":true}}},"layouts":{"list":["id","name","alternativeText","caption"],"edit":[[{"name":"name","size":6},{"name":"alternativeText","size":6}],[{"name":"caption","size":6},{"name":"width","size":4}],[{"name":"height","size":4}],[{"name":"formats","size":12}],[{"name":"hash","size":6},{"name":"ext","size":6}],[{"name":"mime","size":6},{"name":"size","size":4}],[{"name":"url","size":6},{"name":"previewUrl","size":6}],[{"name":"provider","size":6}],[{"name":"provider_metadata","size":12}],[{"name":"folder","size":6},{"name":"folderPath","size":6}]]}}', 'object', NULL, NULL);
INSERT INTO public.strapi_core_store_settings (id, key, value, type, environment, tag) VALUES (15, 'plugin_users-permissions_grant', '{"email":{"enabled":true,"icon":"envelope"},"discord":{"enabled":false,"icon":"discord","key":"","secret":"","callback":"api/auth/discord/callback","scope":["identify","email"]},"facebook":{"enabled":false,"icon":"facebook-square","key":"","secret":"","callback":"api/auth/facebook/callback","scope":["email"]},"google":{"enabled":false,"icon":"google","key":"","secret":"","callback":"api/auth/google/callback","scope":["email"]},"github":{"enabled":false,"icon":"github","key":"","secret":"","callback":"api/auth/github/callback","scope":["user","user:email"]},"microsoft":{"enabled":false,"icon":"windows","key":"","secret":"","callback":"api/auth/microsoft/callback","scope":["user.read"]},"twitter":{"enabled":false,"icon":"twitter","key":"","secret":"","callback":"api/auth/twitter/callback"},"instagram":{"enabled":false,"icon":"instagram","key":"","secret":"","callback":"api/auth/instagram/callback","scope":["user_profile"]},"vk":{"enabled":false,"icon":"vk","key":"","secret":"","callback":"api/auth/vk/callback","scope":["email"]},"twitch":{"enabled":false,"icon":"twitch","key":"","secret":"","callback":"api/auth/twitch/callback","scope":["user:read:email"]},"linkedin":{"enabled":false,"icon":"linkedin","key":"","secret":"","callback":"api/auth/linkedin/callback","scope":["r_liteprofile","r_emailaddress"]},"cognito":{"enabled":false,"icon":"aws","key":"","secret":"","subdomain":"my.subdomain.com","callback":"api/auth/cognito/callback","scope":["email","openid","profile"]},"reddit":{"enabled":false,"icon":"reddit","key":"","secret":"","state":true,"callback":"api/auth/reddit/callback","scope":["identity"]},"auth0":{"enabled":false,"icon":"","key":"","secret":"","subdomain":"my-tenant.eu","callback":"api/auth/auth0/callback","scope":["openid","email","profile"]},"cas":{"enabled":false,"icon":"book","key":"","secret":"","callback":"api/auth/cas/callback","scope":["openid email"],"subdomain":"my.subdomain.com/cas"}}', 'object', NULL, NULL);
INSERT INTO public.strapi_core_store_settings (id, key, value, type, environment, tag) VALUES (16, 'plugin_users-permissions_email', '{"reset_password":{"display":"Email.template.reset_password","icon":"sync","options":{"from":{"name":"Administration Panel","email":"no-reply@strapi.io"},"response_email":"","object":"Reset password","message":"<p>We heard that you lost your password. Sorry about that!</p>\n\n<p>But don’t worry! You can use the following link to reset your password:</p>\n<p><%= URL %>?code=<%= TOKEN %></p>\n\n<p>Thanks.</p>"}},"email_confirmation":{"display":"Email.template.email_confirmation","icon":"check-square","options":{"from":{"name":"Administration Panel","email":"no-reply@strapi.io"},"response_email":"","object":"Account confirmation","message":"<p>Thank you for registering!</p>\n\n<p>You have to confirm your email address. Please click on the link below.</p>\n\n<p><%= URL %>?confirmation=<%= CODE %></p>\n\n<p>Thanks.</p>"}}}', 'object', NULL, NULL);
INSERT INTO public.strapi_core_store_settings (id, key, value, type, environment, tag) VALUES (17, 'plugin_users-permissions_advanced', '{"unique_email":true,"allow_register":true,"email_confirmation":false,"email_reset_password":null,"email_confirmation_redirection":null,"default_role":"authenticated"}', 'object', NULL, NULL);
INSERT INTO public.strapi_core_store_settings (id, key, value, type, environment, tag) VALUES (13, 'plugin_upload_settings', '{"sizeOptimization":false,"responsiveDimensions":false,"autoOrientation":false}', 'object', NULL, NULL);
INSERT INTO public.strapi_core_store_settings (id, key, value, type, environment, tag) VALUES (14, 'plugin_upload_metrics', '{"weeklySchedule":"5 38 6 * * 1","lastWeeklyUpdate":1670827085034}', 'object', NULL, NULL);
INSERT INTO public.strapi_core_store_settings (id, key, value, type, environment, tag) VALUES (11, 'plugin_content_manager_configuration_content_types::plugin::users-permissions.user', '{"uid":"plugin::users-permissions.user","settings":{"bulkable":true,"filterable":true,"searchable":true,"pageSize":10,"mainField":"username","defaultSortBy":"username","defaultSortOrder":"ASC"},"metadatas":{"id":{"edit":{},"list":{"label":"id","searchable":true,"sortable":true}},"username":{"edit":{"label":"username","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"username","searchable":true,"sortable":true}},"email":{"edit":{"label":"email","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"email","searchable":true,"sortable":true}},"provider":{"edit":{"label":"provider","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"provider","searchable":true,"sortable":true}},"password":{"edit":{"label":"password","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"password","searchable":true,"sortable":true}},"resetPasswordToken":{"edit":{"label":"resetPasswordToken","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"resetPasswordToken","searchable":true,"sortable":true}},"confirmationToken":{"edit":{"label":"confirmationToken","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"confirmationToken","searchable":true,"sortable":true}},"confirmed":{"edit":{"label":"confirmed","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"confirmed","searchable":true,"sortable":true}},"blocked":{"edit":{"label":"blocked","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"blocked","searchable":true,"sortable":true}},"role":{"edit":{"label":"role","description":"","placeholder":"","visible":true,"editable":true,"mainField":"name"},"list":{"label":"role","searchable":true,"sortable":true}},"createdAt":{"edit":{"label":"createdAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"createdAt","searchable":true,"sortable":true}},"updatedAt":{"edit":{"label":"updatedAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"updatedAt","searchable":true,"sortable":true}}},"layouts":{"list":["id","username","email","confirmed"],"edit":[[{"name":"username","size":6},{"name":"email","size":6}],[{"name":"password","size":6},{"name":"confirmed","size":4}],[{"name":"blocked","size":4},{"name":"role","size":6}]]}}', 'object', NULL, NULL);
INSERT INTO public.strapi_core_store_settings (id, key, value, type, environment, tag) VALUES (12, 'plugin_content_manager_configuration_content_types::plugin::users-permissions.role', '{"uid":"plugin::users-permissions.role","settings":{"bulkable":true,"filterable":true,"searchable":true,"pageSize":10,"mainField":"name","defaultSortBy":"name","defaultSortOrder":"ASC"},"metadatas":{"id":{"edit":{},"list":{"label":"id","searchable":true,"sortable":true}},"name":{"edit":{"label":"name","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"name","searchable":true,"sortable":true}},"description":{"edit":{"label":"description","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"description","searchable":true,"sortable":true}},"type":{"edit":{"label":"type","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"type","searchable":true,"sortable":true}},"permissions":{"edit":{"label":"permissions","description":"","placeholder":"","visible":true,"editable":true,"mainField":"action"},"list":{"label":"permissions","searchable":false,"sortable":false}},"users":{"edit":{"label":"users","description":"","placeholder":"","visible":true,"editable":true,"mainField":"username"},"list":{"label":"users","searchable":false,"sortable":false}},"createdAt":{"edit":{"label":"createdAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"createdAt","searchable":true,"sortable":true}},"updatedAt":{"edit":{"label":"updatedAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"updatedAt","searchable":true,"sortable":true}}},"layouts":{"list":["id","name","description","type"],"edit":[[{"name":"name","size":6},{"name":"description","size":6}],[{"name":"type","size":6},{"name":"permissions","size":6}],[{"name":"users","size":6}]]}}', 'object', NULL, NULL);
INSERT INTO public.strapi_core_store_settings (id, key, value, type, environment, tag) VALUES (20, 'plugin_content_manager_configuration_components::shared.link', '{"uid":"shared.link","settings":{"bulkable":true,"filterable":true,"searchable":true,"pageSize":10,"mainField":"href","defaultSortBy":"href","defaultSortOrder":"ASC"},"metadatas":{"id":{"edit":{},"list":{"label":"id","searchable":false,"sortable":false}},"href":{"edit":{"label":"href","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"href","searchable":true,"sortable":true}},"label":{"edit":{"label":"label","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"label","searchable":true,"sortable":true}},"target":{"edit":{"label":"target","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"target","searchable":true,"sortable":true}},"isExternal":{"edit":{"label":"isExternal","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"isExternal","searchable":true,"sortable":true}}},"layouts":{"list":["id","href","label","target"],"edit":[[{"name":"href","size":6},{"name":"label","size":6}],[{"name":"target","size":6},{"name":"isExternal","size":4}]]},"isComponent":true}', 'object', NULL, NULL);
INSERT INTO public.strapi_core_store_settings (id, key, value, type, environment, tag) VALUES (21, 'plugin_content_manager_configuration_components::shared.seo', '{"uid":"shared.seo","settings":{"bulkable":true,"filterable":true,"searchable":true,"pageSize":10,"mainField":"metaTitle","defaultSortBy":"metaTitle","defaultSortOrder":"ASC"},"metadatas":{"id":{"edit":{},"list":{"label":"id","searchable":false,"sortable":false}},"metaTitle":{"edit":{"label":"metaTitle","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"metaTitle","searchable":true,"sortable":true}},"metaDescription":{"edit":{"label":"metaDescription","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"metaDescription","searchable":true,"sortable":true}}},"layouts":{"list":["id","metaTitle","metaDescription"],"edit":[[{"name":"metaTitle","size":6},{"name":"metaDescription","size":6}]]},"isComponent":true}', 'object', NULL, NULL);
INSERT INTO public.strapi_core_store_settings (id, key, value, type, environment, tag) VALUES (24, 'plugin_content_manager_configuration_components::blocks.hero-slider-item', '{"uid":"blocks.hero-slider-item","settings":{"bulkable":true,"filterable":true,"searchable":true,"pageSize":10,"mainField":"title","defaultSortBy":"title","defaultSortOrder":"ASC"},"metadatas":{"id":{"edit":{},"list":{"label":"id","searchable":false,"sortable":false}},"image":{"edit":{"label":"image","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"image","searchable":false,"sortable":false}},"title":{"edit":{"label":"title","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"title","searchable":true,"sortable":true}},"text":{"edit":{"label":"text","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"text","searchable":true,"sortable":true}},"cta":{"edit":{"label":"cta","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"cta","searchable":false,"sortable":false}}},"layouts":{"list":["id","title","text","image"],"edit":[[{"name":"title","size":6}],[{"name":"text","size":6}],[{"name":"cta","size":12}],[{"name":"image","size":6}]]},"isComponent":true}', 'object', NULL, NULL);
INSERT INTO public.strapi_core_store_settings (id, key, value, type, environment, tag) VALUES (25, 'plugin_content_manager_configuration_components::blocks.hero-slider', '{"uid":"blocks.hero-slider","settings":{"bulkable":true,"filterable":true,"searchable":true,"pageSize":10,"mainField":"id","defaultSortBy":"id","defaultSortOrder":"ASC"},"metadatas":{"id":{"edit":{},"list":{"label":"id","searchable":false,"sortable":false}},"heroSliderItems":{"edit":{"label":"heroSliderItems","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"heroSliderItems","searchable":false,"sortable":false}}},"layouts":{"list":["id","heroSliderItems"],"edit":[[{"name":"heroSliderItems","size":12}]]},"isComponent":true}', 'object', NULL, NULL);
INSERT INTO public.strapi_core_store_settings (id, key, value, type, environment, tag) VALUES (26, 'plugin_content_manager_configuration_content_types::api::home-page.home-page', '{"uid":"api::home-page.home-page","settings":{"bulkable":true,"filterable":true,"searchable":true,"pageSize":10,"mainField":"id","defaultSortBy":"id","defaultSortOrder":"ASC"},"metadatas":{"id":{"edit":{},"list":{"label":"id","searchable":true,"sortable":true}},"seo":{"edit":{"label":"seo","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"seo","searchable":false,"sortable":false}},"heroSlider":{"edit":{"label":"heroSlider","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"heroSlider","searchable":false,"sortable":false}},"createdAt":{"edit":{"label":"createdAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"createdAt","searchable":true,"sortable":true}},"updatedAt":{"edit":{"label":"updatedAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"updatedAt","searchable":true,"sortable":true}}},"layouts":{"list":["id","seo","heroSlider","createdAt"],"edit":[[{"name":"seo","size":12}],[{"name":"heroSlider","size":12}]]}}', 'object', NULL, NULL);
INSERT INTO public.strapi_core_store_settings (id, key, value, type, environment, tag) VALUES (73, 'plugin_content_manager_configuration_content_types::api::order-page.order-page', '{"uid":"api::order-page.order-page","settings":{"bulkable":true,"filterable":true,"searchable":true,"pageSize":10,"mainField":"id","defaultSortBy":"id","defaultSortOrder":"ASC"},"metadatas":{"id":{"edit":{},"list":{"label":"id","searchable":true,"sortable":true}},"seo":{"edit":{"label":"seo","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"seo","searchable":false,"sortable":false}},"createdAt":{"edit":{"label":"createdAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"createdAt","searchable":true,"sortable":true}},"updatedAt":{"edit":{"label":"updatedAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"updatedAt","searchable":true,"sortable":true}}},"layouts":{"list":["id","seo","createdAt","updatedAt"],"edit":[[{"name":"seo","size":12}]]}}', 'object', NULL, NULL);
INSERT INTO public.strapi_core_store_settings (id, key, value, type, environment, tag) VALUES (74, 'plugin_content_manager_configuration_content_types::api::orders-page.orders-page', '{"uid":"api::orders-page.orders-page","settings":{"bulkable":true,"filterable":true,"searchable":true,"pageSize":10,"mainField":"id","defaultSortBy":"id","defaultSortOrder":"ASC"},"metadatas":{"id":{"edit":{},"list":{"label":"id","searchable":true,"sortable":true}},"seo":{"edit":{"label":"seo","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"seo","searchable":false,"sortable":false}},"createdAt":{"edit":{"label":"createdAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"createdAt","searchable":true,"sortable":true}},"updatedAt":{"edit":{"label":"updatedAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"updatedAt","searchable":true,"sortable":true}}},"layouts":{"list":["id","seo","createdAt","updatedAt"],"edit":[[{"name":"seo","size":12}]]}}', 'object', NULL, NULL);
INSERT INTO public.strapi_core_store_settings (id, key, value, type, environment, tag) VALUES (75, 'plugin_content_manager_configuration_content_types::api::download-center-page.download-center-page', '{"uid":"api::download-center-page.download-center-page","settings":{"bulkable":true,"filterable":true,"searchable":true,"pageSize":10,"mainField":"id","defaultSortBy":"id","defaultSortOrder":"ASC"},"metadatas":{"id":{"edit":{},"list":{"label":"id","searchable":true,"sortable":true}},"seo":{"edit":{"label":"seo","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"seo","searchable":false,"sortable":false}},"createdAt":{"edit":{"label":"createdAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"createdAt","searchable":true,"sortable":true}},"updatedAt":{"edit":{"label":"updatedAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"updatedAt","searchable":true,"sortable":true}}},"layouts":{"list":["id","seo","createdAt","updatedAt"],"edit":[[{"name":"seo","size":12}]]}}', 'object', NULL, NULL);
INSERT INTO public.strapi_core_store_settings (id, key, value, type, environment, tag) VALUES (76, 'plugin_content_manager_configuration_content_types::api::dashboard-page.dashboard-page', '{"uid":"api::dashboard-page.dashboard-page","settings":{"bulkable":true,"filterable":true,"searchable":true,"pageSize":10,"mainField":"id","defaultSortBy":"id","defaultSortOrder":"ASC"},"metadatas":{"id":{"edit":{},"list":{"label":"id","searchable":true,"sortable":true}},"seo":{"edit":{"label":"seo","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"seo","searchable":false,"sortable":false}},"createdAt":{"edit":{"label":"createdAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"createdAt","searchable":true,"sortable":true}},"updatedAt":{"edit":{"label":"updatedAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"updatedAt","searchable":true,"sortable":true}}},"layouts":{"list":["id","seo","createdAt","updatedAt"],"edit":[[{"name":"seo","size":12}]]}}', 'object', NULL, NULL);
INSERT INTO public.strapi_core_store_settings (id, key, value, type, environment, tag) VALUES (78, 'plugin_content_manager_configuration_content_types::api::outlet-page.outlet-page', '{"uid":"api::outlet-page.outlet-page","settings":{"bulkable":true,"filterable":true,"searchable":true,"pageSize":10,"mainField":"id","defaultSortBy":"id","defaultSortOrder":"ASC"},"metadatas":{"id":{"edit":{},"list":{"label":"id","searchable":true,"sortable":true}},"seo":{"edit":{"label":"seo","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"seo","searchable":false,"sortable":false}},"createdAt":{"edit":{"label":"createdAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"createdAt","searchable":true,"sortable":true}},"updatedAt":{"edit":{"label":"updatedAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"updatedAt","searchable":true,"sortable":true}}},"layouts":{"list":["id","seo","createdAt","updatedAt"],"edit":[[{"name":"seo","size":12}]]}}', 'object', NULL, NULL);
INSERT INTO public.strapi_core_store_settings (id, key, value, type, environment, tag) VALUES (79, 'plugin_content_manager_configuration_content_types::api::news-page.news-page', '{"uid":"api::news-page.news-page","settings":{"bulkable":true,"filterable":true,"searchable":true,"pageSize":10,"mainField":"id","defaultSortBy":"id","defaultSortOrder":"ASC"},"metadatas":{"id":{"edit":{},"list":{"label":"id","searchable":true,"sortable":true}},"seo":{"edit":{"label":"seo","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"seo","searchable":false,"sortable":false}},"createdAt":{"edit":{"label":"createdAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"createdAt","searchable":true,"sortable":true}},"updatedAt":{"edit":{"label":"updatedAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"updatedAt","searchable":true,"sortable":true}}},"layouts":{"list":["id","seo","createdAt","updatedAt"],"edit":[[{"name":"seo","size":12}]]}}', 'object', NULL, NULL);
INSERT INTO public.strapi_core_store_settings (id, key, value, type, environment, tag) VALUES (80, 'plugin_content_manager_configuration_content_types::api::available-products-page.available-products-page', '{"uid":"api::available-products-page.available-products-page","settings":{"bulkable":true,"filterable":true,"searchable":true,"pageSize":10,"mainField":"id","defaultSortBy":"id","defaultSortOrder":"ASC"},"metadatas":{"id":{"edit":{},"list":{"label":"id","searchable":true,"sortable":true}},"seo":{"edit":{"label":"seo","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"seo","searchable":false,"sortable":false}},"createdAt":{"edit":{"label":"createdAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"createdAt","searchable":true,"sortable":true}},"updatedAt":{"edit":{"label":"updatedAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"updatedAt","searchable":true,"sortable":true}}},"layouts":{"list":["id","seo","createdAt","updatedAt"],"edit":[[{"name":"seo","size":12}]]}}', 'object', NULL, NULL);
INSERT INTO public.strapi_core_store_settings (id, key, value, type, environment, tag) VALUES (81, 'plugin_content_manager_configuration_content_types::api::regulations-page.regulations-page', '{"uid":"api::regulations-page.regulations-page","settings":{"bulkable":true,"filterable":true,"searchable":true,"pageSize":10,"mainField":"id","defaultSortBy":"id","defaultSortOrder":"ASC"},"metadatas":{"id":{"edit":{},"list":{"label":"id","searchable":true,"sortable":true}},"seo":{"edit":{"label":"seo","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"seo","searchable":false,"sortable":false}},"Text":{"edit":{"label":"Text","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"Text","searchable":false,"sortable":false}},"createdAt":{"edit":{"label":"createdAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"createdAt","searchable":true,"sortable":true}},"updatedAt":{"edit":{"label":"updatedAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"updatedAt","searchable":true,"sortable":true}}},"layouts":{"list":["id","seo","createdAt","updatedAt"],"edit":[[{"name":"seo","size":12}],[{"name":"Text","size":12}]]}}', 'object', NULL, NULL);
INSERT INTO public.strapi_core_store_settings (id, key, value, type, environment, tag) VALUES (82, 'plugin_content_manager_configuration_content_types::api::privacy-policy-page.privacy-policy-page', '{"uid":"api::privacy-policy-page.privacy-policy-page","settings":{"bulkable":true,"filterable":true,"searchable":true,"pageSize":10,"mainField":"id","defaultSortBy":"id","defaultSortOrder":"ASC"},"metadatas":{"id":{"edit":{},"list":{"label":"id","searchable":true,"sortable":true}},"seo":{"edit":{"label":"seo","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"seo","searchable":false,"sortable":false}},"Text":{"edit":{"label":"Text","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"Text","searchable":false,"sortable":false}},"createdAt":{"edit":{"label":"createdAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"createdAt","searchable":true,"sortable":true}},"updatedAt":{"edit":{"label":"updatedAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"updatedAt","searchable":true,"sortable":true}}},"layouts":{"list":["id","seo","createdAt","updatedAt"],"edit":[[{"name":"seo","size":12}],[{"name":"Text","size":12}]]}}', 'object', NULL, NULL);
INSERT INTO public.strapi_core_store_settings (id, key, value, type, environment, tag) VALUES (83, 'plugin_content_manager_configuration_content_types::api::basket-page.basket-page', '{"uid":"api::basket-page.basket-page","settings":{"bulkable":true,"filterable":true,"searchable":true,"pageSize":10,"mainField":"id","defaultSortBy":"id","defaultSortOrder":"ASC"},"metadatas":{"id":{"edit":{},"list":{"label":"id","searchable":true,"sortable":true}},"seo":{"edit":{"label":"seo","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"seo","searchable":false,"sortable":false}},"createdAt":{"edit":{"label":"createdAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"createdAt","searchable":true,"sortable":true}},"updatedAt":{"edit":{"label":"updatedAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"updatedAt","searchable":true,"sortable":true}}},"layouts":{"list":["id","seo","createdAt","updatedAt"],"edit":[[{"name":"seo","size":12}]]}}', 'object', NULL, NULL);
INSERT INTO public.strapi_core_store_settings (id, key, value, type, environment, tag) VALUES (84, 'plugin_content_manager_configuration_content_types::api::order-item-page.order-item-page', '{"uid":"api::order-item-page.order-item-page","settings":{"bulkable":true,"filterable":true,"searchable":true,"pageSize":10,"mainField":"id","defaultSortBy":"id","defaultSortOrder":"ASC"},"metadatas":{"id":{"edit":{},"list":{"label":"id","searchable":true,"sortable":true}},"seo":{"edit":{"label":"seo","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"seo","searchable":false,"sortable":false}},"createdAt":{"edit":{"label":"createdAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"createdAt","searchable":true,"sortable":true}},"updatedAt":{"edit":{"label":"updatedAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"updatedAt","searchable":true,"sortable":true}}},"layouts":{"list":["id","seo","createdAt","updatedAt"],"edit":[[{"name":"seo","size":12}]]}}', 'object', NULL, NULL);
INSERT INTO public.strapi_core_store_settings (id, key, value, type, environment, tag) VALUES (85, 'plugin_content_manager_configuration_content_types::api::search-products-page.search-products-page', '{"uid":"api::search-products-page.search-products-page","settings":{"bulkable":true,"filterable":true,"searchable":true,"pageSize":10,"mainField":"id","defaultSortBy":"id","defaultSortOrder":"ASC"},"metadatas":{"id":{"edit":{},"list":{"label":"id","searchable":true,"sortable":true}},"seo":{"edit":{"label":"seo","description":"","placeholder":"","visible":true,"editable":true},"list":{"label":"seo","searchable":false,"sortable":false}},"createdAt":{"edit":{"label":"createdAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"createdAt","searchable":true,"sortable":true}},"updatedAt":{"edit":{"label":"updatedAt","description":"","placeholder":"","visible":false,"editable":true},"list":{"label":"updatedAt","searchable":true,"sortable":true}}},"layouts":{"list":["id","seo","createdAt","updatedAt"],"edit":[[{"name":"seo","size":12}]]}}', 'object', NULL, NULL);


--
-- TOC entry 3863 (class 0 OID 16536)
-- Dependencies: 240
-- Data for Name: strapi_database_schema; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.strapi_database_schema (id, schema, "time", hash) VALUES (98, '{"tables":[{"name":"strapi_core_store_settings","indexes":[],"foreignKeys":[],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"key","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"value","type":"text","args":["longtext"],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"type","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"environment","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"tag","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false}]},{"name":"strapi_webhooks","indexes":[],"foreignKeys":[],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"name","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"url","type":"text","args":["longtext"],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"headers","type":"jsonb","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"events","type":"jsonb","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"enabled","type":"boolean","args":[],"defaultTo":null,"notNullable":false,"unsigned":false}]},{"name":"admin_permissions","indexes":[{"name":"admin_permissions_created_by_id_fk","columns":["created_by_id"]},{"name":"admin_permissions_updated_by_id_fk","columns":["updated_by_id"]}],"foreignKeys":[{"name":"admin_permissions_created_by_id_fk","columns":["created_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"},{"name":"admin_permissions_updated_by_id_fk","columns":["updated_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"action","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"subject","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"properties","type":"jsonb","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"conditions","type":"jsonb","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"created_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"updated_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"created_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"updated_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"admin_users","indexes":[{"name":"admin_users_created_by_id_fk","columns":["created_by_id"]},{"name":"admin_users_updated_by_id_fk","columns":["updated_by_id"]}],"foreignKeys":[{"name":"admin_users_created_by_id_fk","columns":["created_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"},{"name":"admin_users_updated_by_id_fk","columns":["updated_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"firstname","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"lastname","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"username","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"email","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"password","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"reset_password_token","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"registration_token","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"is_active","type":"boolean","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"blocked","type":"boolean","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"prefered_language","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"created_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"updated_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"created_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"updated_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"admin_roles","indexes":[{"name":"admin_roles_created_by_id_fk","columns":["created_by_id"]},{"name":"admin_roles_updated_by_id_fk","columns":["updated_by_id"]}],"foreignKeys":[{"name":"admin_roles_created_by_id_fk","columns":["created_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"},{"name":"admin_roles_updated_by_id_fk","columns":["updated_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"name","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"code","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"description","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"created_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"updated_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"created_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"updated_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"strapi_api_tokens","indexes":[{"name":"strapi_api_tokens_created_by_id_fk","columns":["created_by_id"]},{"name":"strapi_api_tokens_updated_by_id_fk","columns":["updated_by_id"]}],"foreignKeys":[{"name":"strapi_api_tokens_created_by_id_fk","columns":["created_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"},{"name":"strapi_api_tokens_updated_by_id_fk","columns":["updated_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"name","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"description","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"type","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"access_key","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"last_used_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"expires_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"lifespan","type":"bigInteger","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"created_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"updated_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"created_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"updated_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"strapi_api_token_permissions","indexes":[{"name":"strapi_api_token_permissions_created_by_id_fk","columns":["created_by_id"]},{"name":"strapi_api_token_permissions_updated_by_id_fk","columns":["updated_by_id"]}],"foreignKeys":[{"name":"strapi_api_token_permissions_created_by_id_fk","columns":["created_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"},{"name":"strapi_api_token_permissions_updated_by_id_fk","columns":["updated_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"action","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"created_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"updated_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"created_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"updated_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"files","indexes":[{"name":"upload_files_folder_path_index","columns":["folder_path"],"type":null},{"name":"files_created_by_id_fk","columns":["created_by_id"]},{"name":"files_updated_by_id_fk","columns":["updated_by_id"]}],"foreignKeys":[{"name":"files_created_by_id_fk","columns":["created_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"},{"name":"files_updated_by_id_fk","columns":["updated_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"name","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"alternative_text","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"caption","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"width","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"height","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"formats","type":"jsonb","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"hash","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"ext","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"mime","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"size","type":"decimal","args":[10,2],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"url","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"preview_url","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"provider","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"provider_metadata","type":"jsonb","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"folder_path","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"created_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"updated_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"created_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"updated_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"upload_folders","indexes":[{"name":"upload_folders_path_id_index","columns":["path_id"],"type":"unique"},{"name":"upload_folders_path_index","columns":["path"],"type":"unique"},{"name":"upload_folders_created_by_id_fk","columns":["created_by_id"]},{"name":"upload_folders_updated_by_id_fk","columns":["updated_by_id"]}],"foreignKeys":[{"name":"upload_folders_created_by_id_fk","columns":["created_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"},{"name":"upload_folders_updated_by_id_fk","columns":["updated_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"name","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"path_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"path","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"created_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"updated_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"created_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"updated_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"i18n_locale","indexes":[{"name":"i18n_locale_created_by_id_fk","columns":["created_by_id"]},{"name":"i18n_locale_updated_by_id_fk","columns":["updated_by_id"]}],"foreignKeys":[{"name":"i18n_locale_created_by_id_fk","columns":["created_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"},{"name":"i18n_locale_updated_by_id_fk","columns":["updated_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"name","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"code","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"created_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"updated_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"created_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"updated_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"up_permissions","indexes":[{"name":"up_permissions_created_by_id_fk","columns":["created_by_id"]},{"name":"up_permissions_updated_by_id_fk","columns":["updated_by_id"]}],"foreignKeys":[{"name":"up_permissions_created_by_id_fk","columns":["created_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"},{"name":"up_permissions_updated_by_id_fk","columns":["updated_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"action","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"created_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"updated_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"created_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"updated_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"up_roles","indexes":[{"name":"up_roles_created_by_id_fk","columns":["created_by_id"]},{"name":"up_roles_updated_by_id_fk","columns":["updated_by_id"]}],"foreignKeys":[{"name":"up_roles_created_by_id_fk","columns":["created_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"},{"name":"up_roles_updated_by_id_fk","columns":["updated_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"name","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"description","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"type","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"created_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"updated_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"created_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"updated_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"up_users","indexes":[{"name":"up_users_created_by_id_fk","columns":["created_by_id"]},{"name":"up_users_updated_by_id_fk","columns":["updated_by_id"]}],"foreignKeys":[{"name":"up_users_created_by_id_fk","columns":["created_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"},{"name":"up_users_updated_by_id_fk","columns":["updated_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"username","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"email","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"provider","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"password","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"reset_password_token","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"confirmation_token","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"confirmed","type":"boolean","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"blocked","type":"boolean","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"created_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"updated_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"created_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"updated_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"available_products_pages","indexes":[{"name":"available_products_pages_created_by_id_fk","columns":["created_by_id"]},{"name":"available_products_pages_updated_by_id_fk","columns":["updated_by_id"]}],"foreignKeys":[{"name":"available_products_pages_created_by_id_fk","columns":["created_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"},{"name":"available_products_pages_updated_by_id_fk","columns":["updated_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"created_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"updated_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"published_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"created_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"updated_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"locale","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false}]},{"name":"basket_pages","indexes":[{"name":"basket_pages_created_by_id_fk","columns":["created_by_id"]},{"name":"basket_pages_updated_by_id_fk","columns":["updated_by_id"]}],"foreignKeys":[{"name":"basket_pages_created_by_id_fk","columns":["created_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"},{"name":"basket_pages_updated_by_id_fk","columns":["updated_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"created_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"updated_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"published_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"created_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"updated_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"locale","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false}]},{"name":"dashboard_pages","indexes":[{"name":"dashboard_pages_created_by_id_fk","columns":["created_by_id"]},{"name":"dashboard_pages_updated_by_id_fk","columns":["updated_by_id"]}],"foreignKeys":[{"name":"dashboard_pages_created_by_id_fk","columns":["created_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"},{"name":"dashboard_pages_updated_by_id_fk","columns":["updated_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"created_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"updated_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"published_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"created_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"updated_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"locale","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false}]},{"name":"download_center_pages","indexes":[{"name":"download_center_pages_created_by_id_fk","columns":["created_by_id"]},{"name":"download_center_pages_updated_by_id_fk","columns":["updated_by_id"]}],"foreignKeys":[{"name":"download_center_pages_created_by_id_fk","columns":["created_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"},{"name":"download_center_pages_updated_by_id_fk","columns":["updated_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"created_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"updated_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"published_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"created_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"updated_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"locale","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false}]},{"name":"home_pages","indexes":[{"name":"home_pages_created_by_id_fk","columns":["created_by_id"]},{"name":"home_pages_updated_by_id_fk","columns":["updated_by_id"]}],"foreignKeys":[{"name":"home_pages_created_by_id_fk","columns":["created_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"},{"name":"home_pages_updated_by_id_fk","columns":["updated_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"created_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"updated_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"published_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"created_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"updated_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"locale","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false}]},{"name":"news_pages","indexes":[{"name":"news_pages_created_by_id_fk","columns":["created_by_id"]},{"name":"news_pages_updated_by_id_fk","columns":["updated_by_id"]}],"foreignKeys":[{"name":"news_pages_created_by_id_fk","columns":["created_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"},{"name":"news_pages_updated_by_id_fk","columns":["updated_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"created_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"updated_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"published_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"created_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"updated_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"locale","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false}]},{"name":"order_item_pages","indexes":[{"name":"order_item_pages_created_by_id_fk","columns":["created_by_id"]},{"name":"order_item_pages_updated_by_id_fk","columns":["updated_by_id"]}],"foreignKeys":[{"name":"order_item_pages_created_by_id_fk","columns":["created_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"},{"name":"order_item_pages_updated_by_id_fk","columns":["updated_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"created_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"updated_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"published_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"created_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"updated_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"locale","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false}]},{"name":"order_pages","indexes":[{"name":"order_pages_created_by_id_fk","columns":["created_by_id"]},{"name":"order_pages_updated_by_id_fk","columns":["updated_by_id"]}],"foreignKeys":[{"name":"order_pages_created_by_id_fk","columns":["created_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"},{"name":"order_pages_updated_by_id_fk","columns":["updated_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"created_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"updated_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"published_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"created_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"updated_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"locale","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false}]},{"name":"orders_pages","indexes":[{"name":"orders_pages_created_by_id_fk","columns":["created_by_id"]},{"name":"orders_pages_updated_by_id_fk","columns":["updated_by_id"]}],"foreignKeys":[{"name":"orders_pages_created_by_id_fk","columns":["created_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"},{"name":"orders_pages_updated_by_id_fk","columns":["updated_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"created_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"updated_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"published_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"created_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"updated_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"locale","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false}]},{"name":"outlet_pages","indexes":[{"name":"outlet_pages_created_by_id_fk","columns":["created_by_id"]},{"name":"outlet_pages_updated_by_id_fk","columns":["updated_by_id"]}],"foreignKeys":[{"name":"outlet_pages_created_by_id_fk","columns":["created_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"},{"name":"outlet_pages_updated_by_id_fk","columns":["updated_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"created_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"updated_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"published_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"created_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"updated_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"locale","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false}]},{"name":"privacy_policy_pages","indexes":[{"name":"privacy_policy_pages_created_by_id_fk","columns":["created_by_id"]},{"name":"privacy_policy_pages_updated_by_id_fk","columns":["updated_by_id"]}],"foreignKeys":[{"name":"privacy_policy_pages_created_by_id_fk","columns":["created_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"},{"name":"privacy_policy_pages_updated_by_id_fk","columns":["updated_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"text","type":"text","args":["longtext"],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"created_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"updated_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"published_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"created_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"updated_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"locale","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false}]},{"name":"regulations_pages","indexes":[{"name":"regulations_pages_created_by_id_fk","columns":["created_by_id"]},{"name":"regulations_pages_updated_by_id_fk","columns":["updated_by_id"]}],"foreignKeys":[{"name":"regulations_pages_created_by_id_fk","columns":["created_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"},{"name":"regulations_pages_updated_by_id_fk","columns":["updated_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"text","type":"text","args":["longtext"],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"created_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"updated_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"published_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"created_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"updated_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"locale","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false}]},{"name":"search_products_pages","indexes":[{"name":"search_products_pages_created_by_id_fk","columns":["created_by_id"]},{"name":"search_products_pages_updated_by_id_fk","columns":["updated_by_id"]}],"foreignKeys":[{"name":"search_products_pages_created_by_id_fk","columns":["created_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"},{"name":"search_products_pages_updated_by_id_fk","columns":["updated_by_id"],"referencedTable":"admin_users","referencedColumns":["id"],"onDelete":"SET NULL"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"created_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"updated_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"published_at","type":"datetime","args":[{"useTz":false,"precision":6}],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"created_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"updated_by_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"locale","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false}]},{"name":"components_blocks_hero_slider_items","indexes":[],"foreignKeys":[],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"title","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"text","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false}]},{"name":"components_blocks_hero_sliders","indexes":[],"foreignKeys":[],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false}]},{"name":"components_shared_links","indexes":[],"foreignKeys":[],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"href","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"label","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"target","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"is_external","type":"boolean","args":[],"defaultTo":null,"notNullable":false,"unsigned":false}]},{"name":"components_shared_seos","indexes":[],"foreignKeys":[],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"meta_title","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"meta_description","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false}]},{"name":"admin_permissions_role_links","indexes":[{"name":"admin_permissions_role_links_fk","columns":["permission_id"]},{"name":"admin_permissions_role_links_inv_fk","columns":["role_id"]},{"name":"admin_permissions_role_links_unique","columns":["permission_id","role_id"],"type":"unique"},{"name":"admin_permissions_role_links_order_inv_fk","columns":["permission_order"]}],"foreignKeys":[{"name":"admin_permissions_role_links_fk","columns":["permission_id"],"referencedColumns":["id"],"referencedTable":"admin_permissions","onDelete":"CASCADE"},{"name":"admin_permissions_role_links_inv_fk","columns":["role_id"],"referencedColumns":["id"],"referencedTable":"admin_roles","onDelete":"CASCADE"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"permission_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"role_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"permission_order","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"admin_users_roles_links","indexes":[{"name":"admin_users_roles_links_fk","columns":["user_id"]},{"name":"admin_users_roles_links_inv_fk","columns":["role_id"]},{"name":"admin_users_roles_links_unique","columns":["user_id","role_id"],"type":"unique"},{"name":"admin_users_roles_links_order_fk","columns":["role_order"]},{"name":"admin_users_roles_links_order_inv_fk","columns":["user_order"]}],"foreignKeys":[{"name":"admin_users_roles_links_fk","columns":["user_id"],"referencedColumns":["id"],"referencedTable":"admin_users","onDelete":"CASCADE"},{"name":"admin_users_roles_links_inv_fk","columns":["role_id"],"referencedColumns":["id"],"referencedTable":"admin_roles","onDelete":"CASCADE"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"user_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"role_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"role_order","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"user_order","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"strapi_api_token_permissions_token_links","indexes":[{"name":"strapi_api_token_permissions_token_links_fk","columns":["api_token_permission_id"]},{"name":"strapi_api_token_permissions_token_links_inv_fk","columns":["api_token_id"]},{"name":"strapi_api_token_permissions_token_links_unique","columns":["api_token_permission_id","api_token_id"],"type":"unique"},{"name":"strapi_api_token_permissions_token_links_order_inv_fk","columns":["api_token_permission_order"]}],"foreignKeys":[{"name":"strapi_api_token_permissions_token_links_fk","columns":["api_token_permission_id"],"referencedColumns":["id"],"referencedTable":"strapi_api_token_permissions","onDelete":"CASCADE"},{"name":"strapi_api_token_permissions_token_links_inv_fk","columns":["api_token_id"],"referencedColumns":["id"],"referencedTable":"strapi_api_tokens","onDelete":"CASCADE"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"api_token_permission_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"api_token_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"api_token_permission_order","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"files_related_morphs","indexes":[{"name":"files_related_morphs_fk","columns":["file_id"]}],"foreignKeys":[{"name":"files_related_morphs_fk","columns":["file_id"],"referencedColumns":["id"],"referencedTable":"files","onDelete":"CASCADE"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"file_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"related_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"related_type","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"field","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"order","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"files_folder_links","indexes":[{"name":"files_folder_links_fk","columns":["file_id"]},{"name":"files_folder_links_inv_fk","columns":["folder_id"]},{"name":"files_folder_links_unique","columns":["file_id","folder_id"],"type":"unique"},{"name":"files_folder_links_order_inv_fk","columns":["file_order"]}],"foreignKeys":[{"name":"files_folder_links_fk","columns":["file_id"],"referencedColumns":["id"],"referencedTable":"files","onDelete":"CASCADE"},{"name":"files_folder_links_inv_fk","columns":["folder_id"],"referencedColumns":["id"],"referencedTable":"upload_folders","onDelete":"CASCADE"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"file_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"folder_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"file_order","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"upload_folders_parent_links","indexes":[{"name":"upload_folders_parent_links_fk","columns":["folder_id"]},{"name":"upload_folders_parent_links_inv_fk","columns":["inv_folder_id"]},{"name":"upload_folders_parent_links_unique","columns":["folder_id","inv_folder_id"],"type":"unique"},{"name":"upload_folders_parent_links_order_inv_fk","columns":["folder_order"]}],"foreignKeys":[{"name":"upload_folders_parent_links_fk","columns":["folder_id"],"referencedColumns":["id"],"referencedTable":"upload_folders","onDelete":"CASCADE"},{"name":"upload_folders_parent_links_inv_fk","columns":["inv_folder_id"],"referencedColumns":["id"],"referencedTable":"upload_folders","onDelete":"CASCADE"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"folder_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"inv_folder_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"folder_order","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"up_permissions_role_links","indexes":[{"name":"up_permissions_role_links_fk","columns":["permission_id"]},{"name":"up_permissions_role_links_inv_fk","columns":["role_id"]},{"name":"up_permissions_role_links_unique","columns":["permission_id","role_id"],"type":"unique"},{"name":"up_permissions_role_links_order_inv_fk","columns":["permission_order"]}],"foreignKeys":[{"name":"up_permissions_role_links_fk","columns":["permission_id"],"referencedColumns":["id"],"referencedTable":"up_permissions","onDelete":"CASCADE"},{"name":"up_permissions_role_links_inv_fk","columns":["role_id"],"referencedColumns":["id"],"referencedTable":"up_roles","onDelete":"CASCADE"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"permission_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"role_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"permission_order","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"up_users_role_links","indexes":[{"name":"up_users_role_links_fk","columns":["user_id"]},{"name":"up_users_role_links_inv_fk","columns":["role_id"]},{"name":"up_users_role_links_unique","columns":["user_id","role_id"],"type":"unique"},{"name":"up_users_role_links_order_inv_fk","columns":["user_order"]}],"foreignKeys":[{"name":"up_users_role_links_fk","columns":["user_id"],"referencedColumns":["id"],"referencedTable":"up_users","onDelete":"CASCADE"},{"name":"up_users_role_links_inv_fk","columns":["role_id"],"referencedColumns":["id"],"referencedTable":"up_roles","onDelete":"CASCADE"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"user_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"role_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"user_order","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"available_products_pages_components","indexes":[{"name":"available_products_pages_field_index","columns":["field"],"type":null},{"name":"available_products_pages_component_type_index","columns":["component_type"],"type":null},{"name":"available_products_pages_entity_fk","columns":["entity_id"]},{"name":"available_products_pages_unique","columns":["entity_id","component_id","field","component_type"],"type":"unique"}],"foreignKeys":[{"name":"available_products_pages_entity_fk","columns":["entity_id"],"referencedColumns":["id"],"referencedTable":"available_products_pages","onDelete":"CASCADE"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"entity_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"component_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"component_type","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"field","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"order","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"available_products_pages_localizations_links","indexes":[{"name":"available_products_pages_localizations_links_fk","columns":["available_products_page_id"]},{"name":"available_products_pages_localizations_links_inv_fk","columns":["inv_available_products_page_id"]},{"name":"available_products_pages_localizations_links_unique","columns":["available_products_page_id","inv_available_products_page_id"],"type":"unique"},{"name":"available_products_pages_localizations_links_order_fk","columns":["available_products_page_order"]}],"foreignKeys":[{"name":"available_products_pages_localizations_links_fk","columns":["available_products_page_id"],"referencedColumns":["id"],"referencedTable":"available_products_pages","onDelete":"CASCADE"},{"name":"available_products_pages_localizations_links_inv_fk","columns":["inv_available_products_page_id"],"referencedColumns":["id"],"referencedTable":"available_products_pages","onDelete":"CASCADE"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"available_products_page_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"inv_available_products_page_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"available_products_page_order","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"basket_pages_components","indexes":[{"name":"basket_pages_field_index","columns":["field"],"type":null},{"name":"basket_pages_component_type_index","columns":["component_type"],"type":null},{"name":"basket_pages_entity_fk","columns":["entity_id"]},{"name":"basket_pages_unique","columns":["entity_id","component_id","field","component_type"],"type":"unique"}],"foreignKeys":[{"name":"basket_pages_entity_fk","columns":["entity_id"],"referencedColumns":["id"],"referencedTable":"basket_pages","onDelete":"CASCADE"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"entity_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"component_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"component_type","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"field","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"order","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"basket_pages_localizations_links","indexes":[{"name":"basket_pages_localizations_links_fk","columns":["basket_page_id"]},{"name":"basket_pages_localizations_links_inv_fk","columns":["inv_basket_page_id"]},{"name":"basket_pages_localizations_links_unique","columns":["basket_page_id","inv_basket_page_id"],"type":"unique"},{"name":"basket_pages_localizations_links_order_fk","columns":["basket_page_order"]}],"foreignKeys":[{"name":"basket_pages_localizations_links_fk","columns":["basket_page_id"],"referencedColumns":["id"],"referencedTable":"basket_pages","onDelete":"CASCADE"},{"name":"basket_pages_localizations_links_inv_fk","columns":["inv_basket_page_id"],"referencedColumns":["id"],"referencedTable":"basket_pages","onDelete":"CASCADE"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"basket_page_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"inv_basket_page_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"basket_page_order","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"dashboard_pages_components","indexes":[{"name":"dashboard_pages_field_index","columns":["field"],"type":null},{"name":"dashboard_pages_component_type_index","columns":["component_type"],"type":null},{"name":"dashboard_pages_entity_fk","columns":["entity_id"]},{"name":"dashboard_pages_unique","columns":["entity_id","component_id","field","component_type"],"type":"unique"}],"foreignKeys":[{"name":"dashboard_pages_entity_fk","columns":["entity_id"],"referencedColumns":["id"],"referencedTable":"dashboard_pages","onDelete":"CASCADE"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"entity_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"component_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"component_type","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"field","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"order","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"dashboard_pages_localizations_links","indexes":[{"name":"dashboard_pages_localizations_links_fk","columns":["dashboard_page_id"]},{"name":"dashboard_pages_localizations_links_inv_fk","columns":["inv_dashboard_page_id"]},{"name":"dashboard_pages_localizations_links_unique","columns":["dashboard_page_id","inv_dashboard_page_id"],"type":"unique"},{"name":"dashboard_pages_localizations_links_order_fk","columns":["dashboard_page_order"]}],"foreignKeys":[{"name":"dashboard_pages_localizations_links_fk","columns":["dashboard_page_id"],"referencedColumns":["id"],"referencedTable":"dashboard_pages","onDelete":"CASCADE"},{"name":"dashboard_pages_localizations_links_inv_fk","columns":["inv_dashboard_page_id"],"referencedColumns":["id"],"referencedTable":"dashboard_pages","onDelete":"CASCADE"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"dashboard_page_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"inv_dashboard_page_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"dashboard_page_order","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"download_center_pages_components","indexes":[{"name":"download_center_pages_field_index","columns":["field"],"type":null},{"name":"download_center_pages_component_type_index","columns":["component_type"],"type":null},{"name":"download_center_pages_entity_fk","columns":["entity_id"]},{"name":"download_center_pages_unique","columns":["entity_id","component_id","field","component_type"],"type":"unique"}],"foreignKeys":[{"name":"download_center_pages_entity_fk","columns":["entity_id"],"referencedColumns":["id"],"referencedTable":"download_center_pages","onDelete":"CASCADE"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"entity_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"component_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"component_type","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"field","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"order","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"download_center_pages_localizations_links","indexes":[{"name":"download_center_pages_localizations_links_fk","columns":["download_center_page_id"]},{"name":"download_center_pages_localizations_links_inv_fk","columns":["inv_download_center_page_id"]},{"name":"download_center_pages_localizations_links_unique","columns":["download_center_page_id","inv_download_center_page_id"],"type":"unique"},{"name":"download_center_pages_localizations_links_order_fk","columns":["download_center_page_order"]}],"foreignKeys":[{"name":"download_center_pages_localizations_links_fk","columns":["download_center_page_id"],"referencedColumns":["id"],"referencedTable":"download_center_pages","onDelete":"CASCADE"},{"name":"download_center_pages_localizations_links_inv_fk","columns":["inv_download_center_page_id"],"referencedColumns":["id"],"referencedTable":"download_center_pages","onDelete":"CASCADE"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"download_center_page_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"inv_download_center_page_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"download_center_page_order","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"home_pages_components","indexes":[{"name":"home_pages_field_index","columns":["field"],"type":null},{"name":"home_pages_component_type_index","columns":["component_type"],"type":null},{"name":"home_pages_entity_fk","columns":["entity_id"]},{"name":"home_pages_unique","columns":["entity_id","component_id","field","component_type"],"type":"unique"}],"foreignKeys":[{"name":"home_pages_entity_fk","columns":["entity_id"],"referencedColumns":["id"],"referencedTable":"home_pages","onDelete":"CASCADE"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"entity_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"component_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"component_type","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"field","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"order","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"home_pages_localizations_links","indexes":[{"name":"home_pages_localizations_links_fk","columns":["home_page_id"]},{"name":"home_pages_localizations_links_inv_fk","columns":["inv_home_page_id"]},{"name":"home_pages_localizations_links_unique","columns":["home_page_id","inv_home_page_id"],"type":"unique"},{"name":"home_pages_localizations_links_order_fk","columns":["home_page_order"]}],"foreignKeys":[{"name":"home_pages_localizations_links_fk","columns":["home_page_id"],"referencedColumns":["id"],"referencedTable":"home_pages","onDelete":"CASCADE"},{"name":"home_pages_localizations_links_inv_fk","columns":["inv_home_page_id"],"referencedColumns":["id"],"referencedTable":"home_pages","onDelete":"CASCADE"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"home_page_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"inv_home_page_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"home_page_order","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"news_pages_components","indexes":[{"name":"news_pages_field_index","columns":["field"],"type":null},{"name":"news_pages_component_type_index","columns":["component_type"],"type":null},{"name":"news_pages_entity_fk","columns":["entity_id"]},{"name":"news_pages_unique","columns":["entity_id","component_id","field","component_type"],"type":"unique"}],"foreignKeys":[{"name":"news_pages_entity_fk","columns":["entity_id"],"referencedColumns":["id"],"referencedTable":"news_pages","onDelete":"CASCADE"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"entity_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"component_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"component_type","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"field","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"order","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"news_pages_localizations_links","indexes":[{"name":"news_pages_localizations_links_fk","columns":["news_page_id"]},{"name":"news_pages_localizations_links_inv_fk","columns":["inv_news_page_id"]},{"name":"news_pages_localizations_links_unique","columns":["news_page_id","inv_news_page_id"],"type":"unique"},{"name":"news_pages_localizations_links_order_fk","columns":["news_page_order"]}],"foreignKeys":[{"name":"news_pages_localizations_links_fk","columns":["news_page_id"],"referencedColumns":["id"],"referencedTable":"news_pages","onDelete":"CASCADE"},{"name":"news_pages_localizations_links_inv_fk","columns":["inv_news_page_id"],"referencedColumns":["id"],"referencedTable":"news_pages","onDelete":"CASCADE"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"news_page_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"inv_news_page_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"news_page_order","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"order_item_pages_components","indexes":[{"name":"order_item_pages_field_index","columns":["field"],"type":null},{"name":"order_item_pages_component_type_index","columns":["component_type"],"type":null},{"name":"order_item_pages_entity_fk","columns":["entity_id"]},{"name":"order_item_pages_unique","columns":["entity_id","component_id","field","component_type"],"type":"unique"}],"foreignKeys":[{"name":"order_item_pages_entity_fk","columns":["entity_id"],"referencedColumns":["id"],"referencedTable":"order_item_pages","onDelete":"CASCADE"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"entity_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"component_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"component_type","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"field","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"order","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"order_item_pages_localizations_links","indexes":[{"name":"order_item_pages_localizations_links_fk","columns":["order_item_page_id"]},{"name":"order_item_pages_localizations_links_inv_fk","columns":["inv_order_item_page_id"]},{"name":"order_item_pages_localizations_links_unique","columns":["order_item_page_id","inv_order_item_page_id"],"type":"unique"},{"name":"order_item_pages_localizations_links_order_fk","columns":["order_item_page_order"]}],"foreignKeys":[{"name":"order_item_pages_localizations_links_fk","columns":["order_item_page_id"],"referencedColumns":["id"],"referencedTable":"order_item_pages","onDelete":"CASCADE"},{"name":"order_item_pages_localizations_links_inv_fk","columns":["inv_order_item_page_id"],"referencedColumns":["id"],"referencedTable":"order_item_pages","onDelete":"CASCADE"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"order_item_page_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"inv_order_item_page_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"order_item_page_order","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"order_pages_components","indexes":[{"name":"order_pages_field_index","columns":["field"],"type":null},{"name":"order_pages_component_type_index","columns":["component_type"],"type":null},{"name":"order_pages_entity_fk","columns":["entity_id"]},{"name":"order_pages_unique","columns":["entity_id","component_id","field","component_type"],"type":"unique"}],"foreignKeys":[{"name":"order_pages_entity_fk","columns":["entity_id"],"referencedColumns":["id"],"referencedTable":"order_pages","onDelete":"CASCADE"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"entity_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"component_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"component_type","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"field","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"order","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"order_pages_localizations_links","indexes":[{"name":"order_pages_localizations_links_fk","columns":["order_page_id"]},{"name":"order_pages_localizations_links_inv_fk","columns":["inv_order_page_id"]},{"name":"order_pages_localizations_links_unique","columns":["order_page_id","inv_order_page_id"],"type":"unique"},{"name":"order_pages_localizations_links_order_fk","columns":["order_page_order"]}],"foreignKeys":[{"name":"order_pages_localizations_links_fk","columns":["order_page_id"],"referencedColumns":["id"],"referencedTable":"order_pages","onDelete":"CASCADE"},{"name":"order_pages_localizations_links_inv_fk","columns":["inv_order_page_id"],"referencedColumns":["id"],"referencedTable":"order_pages","onDelete":"CASCADE"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"order_page_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"inv_order_page_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"order_page_order","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"orders_pages_components","indexes":[{"name":"orders_pages_field_index","columns":["field"],"type":null},{"name":"orders_pages_component_type_index","columns":["component_type"],"type":null},{"name":"orders_pages_entity_fk","columns":["entity_id"]},{"name":"orders_pages_unique","columns":["entity_id","component_id","field","component_type"],"type":"unique"}],"foreignKeys":[{"name":"orders_pages_entity_fk","columns":["entity_id"],"referencedColumns":["id"],"referencedTable":"orders_pages","onDelete":"CASCADE"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"entity_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"component_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"component_type","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"field","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"order","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"orders_pages_localizations_links","indexes":[{"name":"orders_pages_localizations_links_fk","columns":["orders_page_id"]},{"name":"orders_pages_localizations_links_inv_fk","columns":["inv_orders_page_id"]},{"name":"orders_pages_localizations_links_unique","columns":["orders_page_id","inv_orders_page_id"],"type":"unique"},{"name":"orders_pages_localizations_links_order_fk","columns":["orders_page_order"]}],"foreignKeys":[{"name":"orders_pages_localizations_links_fk","columns":["orders_page_id"],"referencedColumns":["id"],"referencedTable":"orders_pages","onDelete":"CASCADE"},{"name":"orders_pages_localizations_links_inv_fk","columns":["inv_orders_page_id"],"referencedColumns":["id"],"referencedTable":"orders_pages","onDelete":"CASCADE"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"orders_page_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"inv_orders_page_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"orders_page_order","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"outlet_pages_components","indexes":[{"name":"outlet_pages_field_index","columns":["field"],"type":null},{"name":"outlet_pages_component_type_index","columns":["component_type"],"type":null},{"name":"outlet_pages_entity_fk","columns":["entity_id"]},{"name":"outlet_pages_unique","columns":["entity_id","component_id","field","component_type"],"type":"unique"}],"foreignKeys":[{"name":"outlet_pages_entity_fk","columns":["entity_id"],"referencedColumns":["id"],"referencedTable":"outlet_pages","onDelete":"CASCADE"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"entity_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"component_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"component_type","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"field","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"order","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"outlet_pages_localizations_links","indexes":[{"name":"outlet_pages_localizations_links_fk","columns":["outlet_page_id"]},{"name":"outlet_pages_localizations_links_inv_fk","columns":["inv_outlet_page_id"]},{"name":"outlet_pages_localizations_links_unique","columns":["outlet_page_id","inv_outlet_page_id"],"type":"unique"},{"name":"outlet_pages_localizations_links_order_fk","columns":["outlet_page_order"]}],"foreignKeys":[{"name":"outlet_pages_localizations_links_fk","columns":["outlet_page_id"],"referencedColumns":["id"],"referencedTable":"outlet_pages","onDelete":"CASCADE"},{"name":"outlet_pages_localizations_links_inv_fk","columns":["inv_outlet_page_id"],"referencedColumns":["id"],"referencedTable":"outlet_pages","onDelete":"CASCADE"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"outlet_page_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"inv_outlet_page_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"outlet_page_order","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"privacy_policy_pages_components","indexes":[{"name":"privacy_policy_pages_field_index","columns":["field"],"type":null},{"name":"privacy_policy_pages_component_type_index","columns":["component_type"],"type":null},{"name":"privacy_policy_pages_entity_fk","columns":["entity_id"]},{"name":"privacy_policy_pages_unique","columns":["entity_id","component_id","field","component_type"],"type":"unique"}],"foreignKeys":[{"name":"privacy_policy_pages_entity_fk","columns":["entity_id"],"referencedColumns":["id"],"referencedTable":"privacy_policy_pages","onDelete":"CASCADE"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"entity_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"component_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"component_type","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"field","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"order","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"privacy_policy_pages_localizations_links","indexes":[{"name":"privacy_policy_pages_localizations_links_fk","columns":["privacy_policy_page_id"]},{"name":"privacy_policy_pages_localizations_links_inv_fk","columns":["inv_privacy_policy_page_id"]},{"name":"privacy_policy_pages_localizations_links_unique","columns":["privacy_policy_page_id","inv_privacy_policy_page_id"],"type":"unique"},{"name":"privacy_policy_pages_localizations_links_order_fk","columns":["privacy_policy_page_order"]}],"foreignKeys":[{"name":"privacy_policy_pages_localizations_links_fk","columns":["privacy_policy_page_id"],"referencedColumns":["id"],"referencedTable":"privacy_policy_pages","onDelete":"CASCADE"},{"name":"privacy_policy_pages_localizations_links_inv_fk","columns":["inv_privacy_policy_page_id"],"referencedColumns":["id"],"referencedTable":"privacy_policy_pages","onDelete":"CASCADE"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"privacy_policy_page_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"inv_privacy_policy_page_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"privacy_policy_page_order","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"regulations_pages_components","indexes":[{"name":"regulations_pages_field_index","columns":["field"],"type":null},{"name":"regulations_pages_component_type_index","columns":["component_type"],"type":null},{"name":"regulations_pages_entity_fk","columns":["entity_id"]},{"name":"regulations_pages_unique","columns":["entity_id","component_id","field","component_type"],"type":"unique"}],"foreignKeys":[{"name":"regulations_pages_entity_fk","columns":["entity_id"],"referencedColumns":["id"],"referencedTable":"regulations_pages","onDelete":"CASCADE"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"entity_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"component_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"component_type","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"field","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"order","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"regulations_pages_localizations_links","indexes":[{"name":"regulations_pages_localizations_links_fk","columns":["regulations_page_id"]},{"name":"regulations_pages_localizations_links_inv_fk","columns":["inv_regulations_page_id"]},{"name":"regulations_pages_localizations_links_unique","columns":["regulations_page_id","inv_regulations_page_id"],"type":"unique"},{"name":"regulations_pages_localizations_links_order_fk","columns":["regulations_page_order"]}],"foreignKeys":[{"name":"regulations_pages_localizations_links_fk","columns":["regulations_page_id"],"referencedColumns":["id"],"referencedTable":"regulations_pages","onDelete":"CASCADE"},{"name":"regulations_pages_localizations_links_inv_fk","columns":["inv_regulations_page_id"],"referencedColumns":["id"],"referencedTable":"regulations_pages","onDelete":"CASCADE"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"regulations_page_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"inv_regulations_page_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"regulations_page_order","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"search_products_pages_components","indexes":[{"name":"search_products_pages_field_index","columns":["field"],"type":null},{"name":"search_products_pages_component_type_index","columns":["component_type"],"type":null},{"name":"search_products_pages_entity_fk","columns":["entity_id"]},{"name":"search_products_pages_unique","columns":["entity_id","component_id","field","component_type"],"type":"unique"}],"foreignKeys":[{"name":"search_products_pages_entity_fk","columns":["entity_id"],"referencedColumns":["id"],"referencedTable":"search_products_pages","onDelete":"CASCADE"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"entity_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"component_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"component_type","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"field","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"order","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"search_products_pages_localizations_links","indexes":[{"name":"search_products_pages_localizations_links_fk","columns":["search_products_page_id"]},{"name":"search_products_pages_localizations_links_inv_fk","columns":["inv_search_products_page_id"]},{"name":"search_products_pages_localizations_links_unique","columns":["search_products_page_id","inv_search_products_page_id"],"type":"unique"},{"name":"search_products_pages_localizations_links_order_fk","columns":["search_products_page_order"]}],"foreignKeys":[{"name":"search_products_pages_localizations_links_fk","columns":["search_products_page_id"],"referencedColumns":["id"],"referencedTable":"search_products_pages","onDelete":"CASCADE"},{"name":"search_products_pages_localizations_links_inv_fk","columns":["inv_search_products_page_id"],"referencedColumns":["id"],"referencedTable":"search_products_pages","onDelete":"CASCADE"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"search_products_page_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"inv_search_products_page_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"search_products_page_order","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"components_blocks_hero_slider_items_components","indexes":[{"name":"components_blocks_hero_slider_items_field_index","columns":["field"],"type":null},{"name":"components_blocks_hero_slider_items_component_type_index","columns":["component_type"],"type":null},{"name":"components_blocks_hero_slider_items_entity_fk","columns":["entity_id"]},{"name":"components_blocks_hero_slider_items_unique","columns":["entity_id","component_id","field","component_type"],"type":"unique"}],"foreignKeys":[{"name":"components_blocks_hero_slider_items_entity_fk","columns":["entity_id"],"referencedColumns":["id"],"referencedTable":"components_blocks_hero_slider_items","onDelete":"CASCADE"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"entity_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"component_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"component_type","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"field","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"order","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]},{"name":"components_blocks_hero_sliders_components","indexes":[{"name":"components_blocks_hero_sliders_field_index","columns":["field"],"type":null},{"name":"components_blocks_hero_sliders_component_type_index","columns":["component_type"],"type":null},{"name":"components_blocks_hero_sliders_entity_fk","columns":["entity_id"]},{"name":"components_blocks_hero_sliders_unique","columns":["entity_id","component_id","field","component_type"],"type":"unique"}],"foreignKeys":[{"name":"components_blocks_hero_sliders_entity_fk","columns":["entity_id"],"referencedColumns":["id"],"referencedTable":"components_blocks_hero_sliders","onDelete":"CASCADE"}],"columns":[{"name":"id","type":"increments","args":[{"primary":true,"primaryKey":true}],"defaultTo":null,"notNullable":true,"unsigned":false},{"name":"entity_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"component_id","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true},{"name":"component_type","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"field","type":"string","args":[],"defaultTo":null,"notNullable":false,"unsigned":false},{"name":"order","type":"integer","args":[],"defaultTo":null,"notNullable":false,"unsigned":true}]}]}', '2022-12-14 12:08:56.152', 'f07c4e57c3e126c52ad01b6225c16d26');


--
-- TOC entry 3865 (class 0 OID 16544)
-- Dependencies: 242
-- Data for Name: strapi_migrations; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 3867 (class 0 OID 16549)
-- Dependencies: 244
-- Data for Name: strapi_webhooks; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 3869 (class 0 OID 16557)
-- Dependencies: 246
-- Data for Name: up_permissions; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.up_permissions (id, action, created_at, updated_at, created_by_id, updated_by_id) VALUES (1, 'plugin::users-permissions.user.me', '2022-12-03 18:53:07.783', '2022-12-03 18:53:07.783', NULL, NULL);
INSERT INTO public.up_permissions (id, action, created_at, updated_at, created_by_id, updated_by_id) VALUES (2, 'plugin::users-permissions.auth.changePassword', '2022-12-03 18:53:07.783', '2022-12-03 18:53:07.783', NULL, NULL);
INSERT INTO public.up_permissions (id, action, created_at, updated_at, created_by_id, updated_by_id) VALUES (3, 'plugin::users-permissions.auth.emailConfirmation', '2022-12-03 18:53:07.823', '2022-12-03 18:53:07.823', NULL, NULL);
INSERT INTO public.up_permissions (id, action, created_at, updated_at, created_by_id, updated_by_id) VALUES (4, 'plugin::users-permissions.auth.connect', '2022-12-03 18:53:07.823', '2022-12-03 18:53:07.823', NULL, NULL);
INSERT INTO public.up_permissions (id, action, created_at, updated_at, created_by_id, updated_by_id) VALUES (5, 'plugin::users-permissions.auth.register', '2022-12-03 18:53:07.823', '2022-12-03 18:53:07.823', NULL, NULL);
INSERT INTO public.up_permissions (id, action, created_at, updated_at, created_by_id, updated_by_id) VALUES (6, 'plugin::users-permissions.auth.callback', '2022-12-03 18:53:07.823', '2022-12-03 18:53:07.823', NULL, NULL);
INSERT INTO public.up_permissions (id, action, created_at, updated_at, created_by_id, updated_by_id) VALUES (7, 'plugin::users-permissions.auth.resetPassword', '2022-12-03 18:53:07.823', '2022-12-03 18:53:07.823', NULL, NULL);
INSERT INTO public.up_permissions (id, action, created_at, updated_at, created_by_id, updated_by_id) VALUES (8, 'plugin::users-permissions.auth.sendEmailConfirmation', '2022-12-03 18:53:07.823', '2022-12-03 18:53:07.823', NULL, NULL);
INSERT INTO public.up_permissions (id, action, created_at, updated_at, created_by_id, updated_by_id) VALUES (9, 'plugin::users-permissions.auth.forgotPassword', '2022-12-03 18:53:07.823', '2022-12-03 18:53:07.823', NULL, NULL);


--
-- TOC entry 3871 (class 0 OID 16562)
-- Dependencies: 248
-- Data for Name: up_permissions_role_links; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.up_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (1, 1, 1, 1);
INSERT INTO public.up_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (2, 2, 1, 1);
INSERT INTO public.up_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (3, 5, 2, 1);
INSERT INTO public.up_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (4, 4, 2, 1);
INSERT INTO public.up_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (5, 6, 2, 1);
INSERT INTO public.up_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (6, 3, 2, 1);
INSERT INTO public.up_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (7, 7, 2, 1);
INSERT INTO public.up_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (8, 8, 2, 1);
INSERT INTO public.up_permissions_role_links (id, permission_id, role_id, permission_order) VALUES (9, 9, 2, 1);


--
-- TOC entry 3873 (class 0 OID 16567)
-- Dependencies: 250
-- Data for Name: up_roles; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.up_roles (id, name, description, type, created_at, updated_at, created_by_id, updated_by_id) VALUES (1, 'Authenticated', 'Default role given to authenticated user.', 'authenticated', '2022-12-03 18:53:07.74', '2022-12-03 18:53:07.74', NULL, NULL);
INSERT INTO public.up_roles (id, name, description, type, created_at, updated_at, created_by_id, updated_by_id) VALUES (2, 'Public', 'Default role given to unauthenticated user.', 'public', '2022-12-03 18:53:07.757', '2022-12-03 18:53:07.757', NULL, NULL);


--
-- TOC entry 3875 (class 0 OID 16575)
-- Dependencies: 252
-- Data for Name: up_users; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 3877 (class 0 OID 16583)
-- Dependencies: 254
-- Data for Name: up_users_role_links; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 3879 (class 0 OID 16589)
-- Dependencies: 256
-- Data for Name: upload_folders; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 3881 (class 0 OID 16597)
-- Dependencies: 258
-- Data for Name: upload_folders_parent_links; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4028 (class 0 OID 0)
-- Dependencies: 197
-- Name: admin_permissions_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.admin_permissions_id_seq', 271, true);


--
-- TOC entry 4029 (class 0 OID 0)
-- Dependencies: 199
-- Name: admin_permissions_role_links_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.admin_permissions_role_links_id_seq', 281, true);


--
-- TOC entry 4030 (class 0 OID 0)
-- Dependencies: 201
-- Name: admin_roles_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.admin_roles_id_seq', 3, true);


--
-- TOC entry 4031 (class 0 OID 0)
-- Dependencies: 203
-- Name: admin_users_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.admin_users_id_seq', 1, true);


--
-- TOC entry 4032 (class 0 OID 0)
-- Dependencies: 205
-- Name: admin_users_roles_links_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.admin_users_roles_links_id_seq', 1, true);


--
-- TOC entry 4033 (class 0 OID 0)
-- Dependencies: 298
-- Name: available_products_pages_components_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.available_products_pages_components_id_seq', 5, true);


--
-- TOC entry 4034 (class 0 OID 0)
-- Dependencies: 296
-- Name: available_products_pages_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.available_products_pages_id_seq', 3, true);


--
-- TOC entry 4035 (class 0 OID 0)
-- Dependencies: 300
-- Name: available_products_pages_localizations_links_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.available_products_pages_localizations_links_id_seq', 8, true);


--
-- TOC entry 4036 (class 0 OID 0)
-- Dependencies: 316
-- Name: basket_pages_components_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.basket_pages_components_id_seq', 7, true);


--
-- TOC entry 4037 (class 0 OID 0)
-- Dependencies: 314
-- Name: basket_pages_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.basket_pages_id_seq', 3, true);


--
-- TOC entry 4038 (class 0 OID 0)
-- Dependencies: 318
-- Name: basket_pages_localizations_links_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.basket_pages_localizations_links_id_seq', 8, true);


--
-- TOC entry 4039 (class 0 OID 0)
-- Dependencies: 208
-- Name: components_blocks_hero_slider_items_components_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.components_blocks_hero_slider_items_components_id_seq', 69, true);


--
-- TOC entry 4040 (class 0 OID 0)
-- Dependencies: 209
-- Name: components_blocks_hero_slider_items_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.components_blocks_hero_slider_items_id_seq', 45, true);


--
-- TOC entry 4041 (class 0 OID 0)
-- Dependencies: 212
-- Name: components_blocks_hero_sliders_components_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.components_blocks_hero_sliders_components_id_seq', 70, true);


--
-- TOC entry 4042 (class 0 OID 0)
-- Dependencies: 213
-- Name: components_blocks_hero_sliders_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.components_blocks_hero_sliders_id_seq', 3, true);


--
-- TOC entry 4043 (class 0 OID 0)
-- Dependencies: 215
-- Name: components_shared_links_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.components_shared_links_id_seq', 44, true);


--
-- TOC entry 4044 (class 0 OID 0)
-- Dependencies: 217
-- Name: components_shared_seos_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.components_shared_seos_id_seq', 45, true);


--
-- TOC entry 4045 (class 0 OID 0)
-- Dependencies: 272
-- Name: dashboard_pages_components_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.dashboard_pages_components_id_seq', 3, true);


--
-- TOC entry 4046 (class 0 OID 0)
-- Dependencies: 260
-- Name: dashboard_pages_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.dashboard_pages_id_seq', 3, true);


--
-- TOC entry 4047 (class 0 OID 0)
-- Dependencies: 274
-- Name: dashboard_pages_localizations_links_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.dashboard_pages_localizations_links_id_seq', 8, true);


--
-- TOC entry 4048 (class 0 OID 0)
-- Dependencies: 276
-- Name: download_center_pages_components_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.download_center_pages_components_id_seq', 3, true);


--
-- TOC entry 4049 (class 0 OID 0)
-- Dependencies: 262
-- Name: download_center_pages_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.download_center_pages_id_seq', 3, true);


--
-- TOC entry 4050 (class 0 OID 0)
-- Dependencies: 278
-- Name: download_center_pages_localizations_links_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.download_center_pages_localizations_links_id_seq', 8, true);


--
-- TOC entry 4051 (class 0 OID 0)
-- Dependencies: 220
-- Name: files_folder_links_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.files_folder_links_id_seq', 1, false);


--
-- TOC entry 4052 (class 0 OID 0)
-- Dependencies: 221
-- Name: files_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.files_id_seq', 37, true);


--
-- TOC entry 4053 (class 0 OID 0)
-- Dependencies: 223
-- Name: files_related_morphs_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.files_related_morphs_id_seq', 70, true);


--
-- TOC entry 4054 (class 0 OID 0)
-- Dependencies: 226
-- Name: home_pages_components_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.home_pages_components_id_seq', 50, true);


--
-- TOC entry 4055 (class 0 OID 0)
-- Dependencies: 227
-- Name: home_pages_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.home_pages_id_seq', 3, true);


--
-- TOC entry 4056 (class 0 OID 0)
-- Dependencies: 229
-- Name: home_pages_localizations_links_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.home_pages_localizations_links_id_seq', 8, true);


--
-- TOC entry 4057 (class 0 OID 0)
-- Dependencies: 231
-- Name: i18n_locale_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.i18n_locale_id_seq', 3, true);


--
-- TOC entry 4058 (class 0 OID 0)
-- Dependencies: 280
-- Name: news_pages_components_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.news_pages_components_id_seq', 3, true);


--
-- TOC entry 4059 (class 0 OID 0)
-- Dependencies: 264
-- Name: news_pages_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.news_pages_id_seq', 3, true);


--
-- TOC entry 4060 (class 0 OID 0)
-- Dependencies: 282
-- Name: news_pages_localizations_links_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.news_pages_localizations_links_id_seq', 8, true);


--
-- TOC entry 4061 (class 0 OID 0)
-- Dependencies: 322
-- Name: order_item_pages_components_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.order_item_pages_components_id_seq', 3, true);


--
-- TOC entry 4062 (class 0 OID 0)
-- Dependencies: 320
-- Name: order_item_pages_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.order_item_pages_id_seq', 3, true);


--
-- TOC entry 4063 (class 0 OID 0)
-- Dependencies: 324
-- Name: order_item_pages_localizations_links_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.order_item_pages_localizations_links_id_seq', 8, true);


--
-- TOC entry 4064 (class 0 OID 0)
-- Dependencies: 284
-- Name: order_pages_components_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.order_pages_components_id_seq', 3, true);


--
-- TOC entry 4065 (class 0 OID 0)
-- Dependencies: 266
-- Name: order_pages_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.order_pages_id_seq', 3, true);


--
-- TOC entry 4066 (class 0 OID 0)
-- Dependencies: 286
-- Name: order_pages_localizations_links_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.order_pages_localizations_links_id_seq', 8, true);


--
-- TOC entry 4067 (class 0 OID 0)
-- Dependencies: 288
-- Name: orders_pages_components_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.orders_pages_components_id_seq', 5, true);


--
-- TOC entry 4068 (class 0 OID 0)
-- Dependencies: 268
-- Name: orders_pages_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.orders_pages_id_seq', 3, true);


--
-- TOC entry 4069 (class 0 OID 0)
-- Dependencies: 290
-- Name: orders_pages_localizations_links_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.orders_pages_localizations_links_id_seq', 8, true);


--
-- TOC entry 4070 (class 0 OID 0)
-- Dependencies: 292
-- Name: outlet_pages_components_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.outlet_pages_components_id_seq', 3, true);


--
-- TOC entry 4071 (class 0 OID 0)
-- Dependencies: 270
-- Name: outlet_pages_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.outlet_pages_id_seq', 3, true);


--
-- TOC entry 4072 (class 0 OID 0)
-- Dependencies: 294
-- Name: outlet_pages_localizations_links_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.outlet_pages_localizations_links_id_seq', 8, true);


--
-- TOC entry 4073 (class 0 OID 0)
-- Dependencies: 310
-- Name: privacy_policy_pages_components_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.privacy_policy_pages_components_id_seq', 3, true);


--
-- TOC entry 4074 (class 0 OID 0)
-- Dependencies: 308
-- Name: privacy_policy_pages_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.privacy_policy_pages_id_seq', 3, true);


--
-- TOC entry 4075 (class 0 OID 0)
-- Dependencies: 312
-- Name: privacy_policy_pages_localizations_links_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.privacy_policy_pages_localizations_links_id_seq', 8, true);


--
-- TOC entry 4076 (class 0 OID 0)
-- Dependencies: 304
-- Name: regulations_pages_components_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.regulations_pages_components_id_seq', 4, true);


--
-- TOC entry 4077 (class 0 OID 0)
-- Dependencies: 302
-- Name: regulations_pages_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.regulations_pages_id_seq', 3, true);


--
-- TOC entry 4078 (class 0 OID 0)
-- Dependencies: 306
-- Name: regulations_pages_localizations_links_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.regulations_pages_localizations_links_id_seq', 8, true);


--
-- TOC entry 4079 (class 0 OID 0)
-- Dependencies: 328
-- Name: search_products_pages_components_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.search_products_pages_components_id_seq', 3, true);


--
-- TOC entry 4080 (class 0 OID 0)
-- Dependencies: 326
-- Name: search_products_pages_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.search_products_pages_id_seq', 3, true);


--
-- TOC entry 4081 (class 0 OID 0)
-- Dependencies: 330
-- Name: search_products_pages_localizations_links_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.search_products_pages_localizations_links_id_seq', 8, true);


--
-- TOC entry 4082 (class 0 OID 0)
-- Dependencies: 233
-- Name: strapi_api_token_permissions_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.strapi_api_token_permissions_id_seq', 1, false);


--
-- TOC entry 4083 (class 0 OID 0)
-- Dependencies: 235
-- Name: strapi_api_token_permissions_token_links_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.strapi_api_token_permissions_token_links_id_seq', 1, false);


--
-- TOC entry 4084 (class 0 OID 0)
-- Dependencies: 237
-- Name: strapi_api_tokens_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.strapi_api_tokens_id_seq', 1, true);


--
-- TOC entry 4085 (class 0 OID 0)
-- Dependencies: 239
-- Name: strapi_core_store_settings_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.strapi_core_store_settings_id_seq', 85, true);


--
-- TOC entry 4086 (class 0 OID 0)
-- Dependencies: 241
-- Name: strapi_database_schema_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.strapi_database_schema_id_seq', 98, true);


--
-- TOC entry 4087 (class 0 OID 0)
-- Dependencies: 243
-- Name: strapi_migrations_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.strapi_migrations_id_seq', 1, false);


--
-- TOC entry 4088 (class 0 OID 0)
-- Dependencies: 245
-- Name: strapi_webhooks_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.strapi_webhooks_id_seq', 1, false);


--
-- TOC entry 4089 (class 0 OID 0)
-- Dependencies: 247
-- Name: up_permissions_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.up_permissions_id_seq', 9, true);


--
-- TOC entry 4090 (class 0 OID 0)
-- Dependencies: 249
-- Name: up_permissions_role_links_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.up_permissions_role_links_id_seq', 9, true);


--
-- TOC entry 4091 (class 0 OID 0)
-- Dependencies: 251
-- Name: up_roles_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.up_roles_id_seq', 2, true);


--
-- TOC entry 4092 (class 0 OID 0)
-- Dependencies: 253
-- Name: up_users_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.up_users_id_seq', 1, false);


--
-- TOC entry 4093 (class 0 OID 0)
-- Dependencies: 255
-- Name: up_users_role_links_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.up_users_role_links_id_seq', 1, false);


--
-- TOC entry 4094 (class 0 OID 0)
-- Dependencies: 257
-- Name: upload_folders_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.upload_folders_id_seq', 1, false);


--
-- TOC entry 4095 (class 0 OID 0)
-- Dependencies: 259
-- Name: upload_folders_parent_links_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.upload_folders_parent_links_id_seq', 1, false);


--
-- TOC entry 3230 (class 2606 OID 16637)
-- Name: admin_permissions admin_permissions_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.admin_permissions
    ADD CONSTRAINT admin_permissions_pkey PRIMARY KEY (id);


--
-- TOC entry 3236 (class 2606 OID 16639)
-- Name: admin_permissions_role_links admin_permissions_role_links_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.admin_permissions_role_links
    ADD CONSTRAINT admin_permissions_role_links_pkey PRIMARY KEY (id);


--
-- TOC entry 3238 (class 2606 OID 16641)
-- Name: admin_permissions_role_links admin_permissions_role_links_unique; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.admin_permissions_role_links
    ADD CONSTRAINT admin_permissions_role_links_unique UNIQUE (permission_id, role_id);


--
-- TOC entry 3241 (class 2606 OID 16643)
-- Name: admin_roles admin_roles_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.admin_roles
    ADD CONSTRAINT admin_roles_pkey PRIMARY KEY (id);


--
-- TOC entry 3245 (class 2606 OID 16645)
-- Name: admin_users admin_users_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.admin_users
    ADD CONSTRAINT admin_users_pkey PRIMARY KEY (id);


--
-- TOC entry 3252 (class 2606 OID 16647)
-- Name: admin_users_roles_links admin_users_roles_links_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.admin_users_roles_links
    ADD CONSTRAINT admin_users_roles_links_pkey PRIMARY KEY (id);


--
-- TOC entry 3254 (class 2606 OID 16649)
-- Name: admin_users_roles_links admin_users_roles_links_unique; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.admin_users_roles_links
    ADD CONSTRAINT admin_users_roles_links_unique UNIQUE (user_id, role_id);


--
-- TOC entry 3492 (class 2606 OID 214113)
-- Name: available_products_pages_components available_products_pages_components_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.available_products_pages_components
    ADD CONSTRAINT available_products_pages_components_pkey PRIMARY KEY (id);


--
-- TOC entry 3501 (class 2606 OID 214126)
-- Name: available_products_pages_localizations_links available_products_pages_localizations_links_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.available_products_pages_localizations_links
    ADD CONSTRAINT available_products_pages_localizations_links_pkey PRIMARY KEY (id);


--
-- TOC entry 3503 (class 2606 OID 214130)
-- Name: available_products_pages_localizations_links available_products_pages_localizations_links_unique; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.available_products_pages_localizations_links
    ADD CONSTRAINT available_products_pages_localizations_links_unique UNIQUE (available_products_page_id, inv_available_products_page_id);


--
-- TOC entry 3488 (class 2606 OID 214100)
-- Name: available_products_pages available_products_pages_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.available_products_pages
    ADD CONSTRAINT available_products_pages_pkey PRIMARY KEY (id);


--
-- TOC entry 3496 (class 2606 OID 214660)
-- Name: available_products_pages_components available_products_pages_unique; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.available_products_pages_components
    ADD CONSTRAINT available_products_pages_unique UNIQUE (entity_id, component_id, field, component_type);


--
-- TOC entry 3546 (class 2606 OID 214432)
-- Name: basket_pages_components basket_pages_components_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.basket_pages_components
    ADD CONSTRAINT basket_pages_components_pkey PRIMARY KEY (id);


--
-- TOC entry 3555 (class 2606 OID 214445)
-- Name: basket_pages_localizations_links basket_pages_localizations_links_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.basket_pages_localizations_links
    ADD CONSTRAINT basket_pages_localizations_links_pkey PRIMARY KEY (id);


--
-- TOC entry 3557 (class 2606 OID 214449)
-- Name: basket_pages_localizations_links basket_pages_localizations_links_unique; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.basket_pages_localizations_links
    ADD CONSTRAINT basket_pages_localizations_links_unique UNIQUE (basket_page_id, inv_basket_page_id);


--
-- TOC entry 3542 (class 2606 OID 214419)
-- Name: basket_pages basket_pages_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.basket_pages
    ADD CONSTRAINT basket_pages_pkey PRIMARY KEY (id);


--
-- TOC entry 3550 (class 2606 OID 214662)
-- Name: basket_pages_components basket_pages_unique; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.basket_pages_components
    ADD CONSTRAINT basket_pages_unique UNIQUE (entity_id, component_id, field, component_type);


--
-- TOC entry 3259 (class 2606 OID 16651)
-- Name: components_blocks_hero_slider_items_components components_blocks_hero_slider_items_components_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.components_blocks_hero_slider_items_components
    ADD CONSTRAINT components_blocks_hero_slider_items_components_pkey PRIMARY KEY (id);


--
-- TOC entry 3256 (class 2606 OID 16653)
-- Name: components_blocks_hero_slider_items components_blocks_hero_slider_items_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.components_blocks_hero_slider_items
    ADD CONSTRAINT components_blocks_hero_slider_items_pkey PRIMARY KEY (id);


--
-- TOC entry 3263 (class 2606 OID 214684)
-- Name: components_blocks_hero_slider_items_components components_blocks_hero_slider_items_unique; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.components_blocks_hero_slider_items_components
    ADD CONSTRAINT components_blocks_hero_slider_items_unique UNIQUE (entity_id, component_id, field, component_type);


--
-- TOC entry 3268 (class 2606 OID 16657)
-- Name: components_blocks_hero_sliders_components components_blocks_hero_sliders_components_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.components_blocks_hero_sliders_components
    ADD CONSTRAINT components_blocks_hero_sliders_components_pkey PRIMARY KEY (id);


--
-- TOC entry 3265 (class 2606 OID 16659)
-- Name: components_blocks_hero_sliders components_blocks_hero_sliders_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.components_blocks_hero_sliders
    ADD CONSTRAINT components_blocks_hero_sliders_pkey PRIMARY KEY (id);


--
-- TOC entry 3272 (class 2606 OID 214686)
-- Name: components_blocks_hero_sliders_components components_blocks_hero_sliders_unique; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.components_blocks_hero_sliders_components
    ADD CONSTRAINT components_blocks_hero_sliders_unique UNIQUE (entity_id, component_id, field, component_type);


--
-- TOC entry 3274 (class 2606 OID 16663)
-- Name: components_shared_links components_shared_links_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.components_shared_links
    ADD CONSTRAINT components_shared_links_pkey PRIMARY KEY (id);


--
-- TOC entry 3276 (class 2606 OID 16665)
-- Name: components_shared_seos components_shared_seos_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.components_shared_seos
    ADD CONSTRAINT components_shared_seos_pkey PRIMARY KEY (id);


--
-- TOC entry 3404 (class 2606 OID 213696)
-- Name: dashboard_pages_components dashboard_pages_components_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.dashboard_pages_components
    ADD CONSTRAINT dashboard_pages_components_pkey PRIMARY KEY (id);


--
-- TOC entry 3413 (class 2606 OID 213709)
-- Name: dashboard_pages_localizations_links dashboard_pages_localizations_links_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.dashboard_pages_localizations_links
    ADD CONSTRAINT dashboard_pages_localizations_links_pkey PRIMARY KEY (id);


--
-- TOC entry 3415 (class 2606 OID 213713)
-- Name: dashboard_pages_localizations_links dashboard_pages_localizations_links_unique; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.dashboard_pages_localizations_links
    ADD CONSTRAINT dashboard_pages_localizations_links_unique UNIQUE (dashboard_page_id, inv_dashboard_page_id);


--
-- TOC entry 3380 (class 2606 OID 213623)
-- Name: dashboard_pages dashboard_pages_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.dashboard_pages
    ADD CONSTRAINT dashboard_pages_pkey PRIMARY KEY (id);


--
-- TOC entry 3408 (class 2606 OID 214664)
-- Name: dashboard_pages_components dashboard_pages_unique; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.dashboard_pages_components
    ADD CONSTRAINT dashboard_pages_unique UNIQUE (entity_id, component_id, field, component_type);


--
-- TOC entry 3418 (class 2606 OID 213725)
-- Name: download_center_pages_components download_center_pages_components_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.download_center_pages_components
    ADD CONSTRAINT download_center_pages_components_pkey PRIMARY KEY (id);


--
-- TOC entry 3427 (class 2606 OID 213738)
-- Name: download_center_pages_localizations_links download_center_pages_localizations_links_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.download_center_pages_localizations_links
    ADD CONSTRAINT download_center_pages_localizations_links_pkey PRIMARY KEY (id);


--
-- TOC entry 3429 (class 2606 OID 213742)
-- Name: download_center_pages_localizations_links download_center_pages_localizations_links_unique; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.download_center_pages_localizations_links
    ADD CONSTRAINT download_center_pages_localizations_links_unique UNIQUE (download_center_page_id, inv_download_center_page_id);


--
-- TOC entry 3384 (class 2606 OID 213633)
-- Name: download_center_pages download_center_pages_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.download_center_pages
    ADD CONSTRAINT download_center_pages_pkey PRIMARY KEY (id);


--
-- TOC entry 3422 (class 2606 OID 214666)
-- Name: download_center_pages_components download_center_pages_unique; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.download_center_pages_components
    ADD CONSTRAINT download_center_pages_unique UNIQUE (entity_id, component_id, field, component_type);


--
-- TOC entry 3286 (class 2606 OID 16667)
-- Name: files_folder_links files_folder_links_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.files_folder_links
    ADD CONSTRAINT files_folder_links_pkey PRIMARY KEY (id);


--
-- TOC entry 3288 (class 2606 OID 16669)
-- Name: files_folder_links files_folder_links_unique; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.files_folder_links
    ADD CONSTRAINT files_folder_links_unique UNIQUE (file_id, folder_id);


--
-- TOC entry 3279 (class 2606 OID 16671)
-- Name: files files_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.files
    ADD CONSTRAINT files_pkey PRIMARY KEY (id);


--
-- TOC entry 3291 (class 2606 OID 16673)
-- Name: files_related_morphs files_related_morphs_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.files_related_morphs
    ADD CONSTRAINT files_related_morphs_pkey PRIMARY KEY (id);


--
-- TOC entry 3298 (class 2606 OID 16675)
-- Name: home_pages_components home_pages_components_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.home_pages_components
    ADD CONSTRAINT home_pages_components_pkey PRIMARY KEY (id);


--
-- TOC entry 3307 (class 2606 OID 16677)
-- Name: home_pages_localizations_links home_pages_localizations_links_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.home_pages_localizations_links
    ADD CONSTRAINT home_pages_localizations_links_pkey PRIMARY KEY (id);


--
-- TOC entry 3309 (class 2606 OID 16679)
-- Name: home_pages_localizations_links home_pages_localizations_links_unique; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.home_pages_localizations_links
    ADD CONSTRAINT home_pages_localizations_links_unique UNIQUE (home_page_id, inv_home_page_id);


--
-- TOC entry 3294 (class 2606 OID 16681)
-- Name: home_pages home_pages_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.home_pages
    ADD CONSTRAINT home_pages_pkey PRIMARY KEY (id);


--
-- TOC entry 3302 (class 2606 OID 214668)
-- Name: home_pages_components home_pages_unique; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.home_pages_components
    ADD CONSTRAINT home_pages_unique UNIQUE (entity_id, component_id, field, component_type);


--
-- TOC entry 3312 (class 2606 OID 16685)
-- Name: i18n_locale i18n_locale_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.i18n_locale
    ADD CONSTRAINT i18n_locale_pkey PRIMARY KEY (id);


--
-- TOC entry 3432 (class 2606 OID 213783)
-- Name: news_pages_components news_pages_components_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.news_pages_components
    ADD CONSTRAINT news_pages_components_pkey PRIMARY KEY (id);


--
-- TOC entry 3441 (class 2606 OID 213796)
-- Name: news_pages_localizations_links news_pages_localizations_links_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.news_pages_localizations_links
    ADD CONSTRAINT news_pages_localizations_links_pkey PRIMARY KEY (id);


--
-- TOC entry 3443 (class 2606 OID 213800)
-- Name: news_pages_localizations_links news_pages_localizations_links_unique; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.news_pages_localizations_links
    ADD CONSTRAINT news_pages_localizations_links_unique UNIQUE (news_page_id, inv_news_page_id);


--
-- TOC entry 3388 (class 2606 OID 213653)
-- Name: news_pages news_pages_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.news_pages
    ADD CONSTRAINT news_pages_pkey PRIMARY KEY (id);


--
-- TOC entry 3436 (class 2606 OID 214670)
-- Name: news_pages_components news_pages_unique; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.news_pages_components
    ADD CONSTRAINT news_pages_unique UNIQUE (entity_id, component_id, field, component_type);


--
-- TOC entry 3564 (class 2606 OID 214523)
-- Name: order_item_pages_components order_item_pages_components_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_item_pages_components
    ADD CONSTRAINT order_item_pages_components_pkey PRIMARY KEY (id);


--
-- TOC entry 3573 (class 2606 OID 214536)
-- Name: order_item_pages_localizations_links order_item_pages_localizations_links_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_item_pages_localizations_links
    ADD CONSTRAINT order_item_pages_localizations_links_pkey PRIMARY KEY (id);


--
-- TOC entry 3575 (class 2606 OID 214540)
-- Name: order_item_pages_localizations_links order_item_pages_localizations_links_unique; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_item_pages_localizations_links
    ADD CONSTRAINT order_item_pages_localizations_links_unique UNIQUE (order_item_page_id, inv_order_item_page_id);


--
-- TOC entry 3560 (class 2606 OID 214510)
-- Name: order_item_pages order_item_pages_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_item_pages
    ADD CONSTRAINT order_item_pages_pkey PRIMARY KEY (id);


--
-- TOC entry 3568 (class 2606 OID 214672)
-- Name: order_item_pages_components order_item_pages_unique; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_item_pages_components
    ADD CONSTRAINT order_item_pages_unique UNIQUE (entity_id, component_id, field, component_type);


--
-- TOC entry 3446 (class 2606 OID 213812)
-- Name: order_pages_components order_pages_components_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_pages_components
    ADD CONSTRAINT order_pages_components_pkey PRIMARY KEY (id);


--
-- TOC entry 3455 (class 2606 OID 213825)
-- Name: order_pages_localizations_links order_pages_localizations_links_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_pages_localizations_links
    ADD CONSTRAINT order_pages_localizations_links_pkey PRIMARY KEY (id);


--
-- TOC entry 3457 (class 2606 OID 213829)
-- Name: order_pages_localizations_links order_pages_localizations_links_unique; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_pages_localizations_links
    ADD CONSTRAINT order_pages_localizations_links_unique UNIQUE (order_page_id, inv_order_page_id);


--
-- TOC entry 3392 (class 2606 OID 213663)
-- Name: order_pages order_pages_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_pages
    ADD CONSTRAINT order_pages_pkey PRIMARY KEY (id);


--
-- TOC entry 3450 (class 2606 OID 214674)
-- Name: order_pages_components order_pages_unique; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_pages_components
    ADD CONSTRAINT order_pages_unique UNIQUE (entity_id, component_id, field, component_type);


--
-- TOC entry 3460 (class 2606 OID 213841)
-- Name: orders_pages_components orders_pages_components_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orders_pages_components
    ADD CONSTRAINT orders_pages_components_pkey PRIMARY KEY (id);


--
-- TOC entry 3469 (class 2606 OID 213854)
-- Name: orders_pages_localizations_links orders_pages_localizations_links_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orders_pages_localizations_links
    ADD CONSTRAINT orders_pages_localizations_links_pkey PRIMARY KEY (id);


--
-- TOC entry 3471 (class 2606 OID 213858)
-- Name: orders_pages_localizations_links orders_pages_localizations_links_unique; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orders_pages_localizations_links
    ADD CONSTRAINT orders_pages_localizations_links_unique UNIQUE (orders_page_id, inv_orders_page_id);


--
-- TOC entry 3396 (class 2606 OID 213673)
-- Name: orders_pages orders_pages_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orders_pages
    ADD CONSTRAINT orders_pages_pkey PRIMARY KEY (id);


--
-- TOC entry 3464 (class 2606 OID 214676)
-- Name: orders_pages_components orders_pages_unique; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orders_pages_components
    ADD CONSTRAINT orders_pages_unique UNIQUE (entity_id, component_id, field, component_type);


--
-- TOC entry 3474 (class 2606 OID 213870)
-- Name: outlet_pages_components outlet_pages_components_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.outlet_pages_components
    ADD CONSTRAINT outlet_pages_components_pkey PRIMARY KEY (id);


--
-- TOC entry 3483 (class 2606 OID 213883)
-- Name: outlet_pages_localizations_links outlet_pages_localizations_links_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.outlet_pages_localizations_links
    ADD CONSTRAINT outlet_pages_localizations_links_pkey PRIMARY KEY (id);


--
-- TOC entry 3485 (class 2606 OID 213887)
-- Name: outlet_pages_localizations_links outlet_pages_localizations_links_unique; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.outlet_pages_localizations_links
    ADD CONSTRAINT outlet_pages_localizations_links_unique UNIQUE (outlet_page_id, inv_outlet_page_id);


--
-- TOC entry 3400 (class 2606 OID 213683)
-- Name: outlet_pages outlet_pages_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.outlet_pages
    ADD CONSTRAINT outlet_pages_pkey PRIMARY KEY (id);


--
-- TOC entry 3478 (class 2606 OID 214678)
-- Name: outlet_pages_components outlet_pages_unique; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.outlet_pages_components
    ADD CONSTRAINT outlet_pages_unique UNIQUE (entity_id, component_id, field, component_type);


--
-- TOC entry 3528 (class 2606 OID 214292)
-- Name: privacy_policy_pages_components privacy_policy_pages_components_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.privacy_policy_pages_components
    ADD CONSTRAINT privacy_policy_pages_components_pkey PRIMARY KEY (id);


--
-- TOC entry 3537 (class 2606 OID 214305)
-- Name: privacy_policy_pages_localizations_links privacy_policy_pages_localizations_links_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.privacy_policy_pages_localizations_links
    ADD CONSTRAINT privacy_policy_pages_localizations_links_pkey PRIMARY KEY (id);


--
-- TOC entry 3539 (class 2606 OID 214309)
-- Name: privacy_policy_pages_localizations_links privacy_policy_pages_localizations_links_unique; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.privacy_policy_pages_localizations_links
    ADD CONSTRAINT privacy_policy_pages_localizations_links_unique UNIQUE (privacy_policy_page_id, inv_privacy_policy_page_id);


--
-- TOC entry 3524 (class 2606 OID 214279)
-- Name: privacy_policy_pages privacy_policy_pages_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.privacy_policy_pages
    ADD CONSTRAINT privacy_policy_pages_pkey PRIMARY KEY (id);


--
-- TOC entry 3532 (class 2606 OID 214680)
-- Name: privacy_policy_pages_components privacy_policy_pages_unique; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.privacy_policy_pages_components
    ADD CONSTRAINT privacy_policy_pages_unique UNIQUE (entity_id, component_id, field, component_type);


--
-- TOC entry 3510 (class 2606 OID 214202)
-- Name: regulations_pages_components regulations_pages_components_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.regulations_pages_components
    ADD CONSTRAINT regulations_pages_components_pkey PRIMARY KEY (id);


--
-- TOC entry 3519 (class 2606 OID 214215)
-- Name: regulations_pages_localizations_links regulations_pages_localizations_links_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.regulations_pages_localizations_links
    ADD CONSTRAINT regulations_pages_localizations_links_pkey PRIMARY KEY (id);


--
-- TOC entry 3521 (class 2606 OID 214219)
-- Name: regulations_pages_localizations_links regulations_pages_localizations_links_unique; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.regulations_pages_localizations_links
    ADD CONSTRAINT regulations_pages_localizations_links_unique UNIQUE (regulations_page_id, inv_regulations_page_id);


--
-- TOC entry 3506 (class 2606 OID 214189)
-- Name: regulations_pages regulations_pages_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.regulations_pages
    ADD CONSTRAINT regulations_pages_pkey PRIMARY KEY (id);


--
-- TOC entry 3514 (class 2606 OID 214682)
-- Name: regulations_pages_components regulations_pages_unique; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.regulations_pages_components
    ADD CONSTRAINT regulations_pages_unique UNIQUE (entity_id, component_id, field, component_type);


--
-- TOC entry 3582 (class 2606 OID 214615)
-- Name: search_products_pages_components search_products_pages_components_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.search_products_pages_components
    ADD CONSTRAINT search_products_pages_components_pkey PRIMARY KEY (id);


--
-- TOC entry 3591 (class 2606 OID 214628)
-- Name: search_products_pages_localizations_links search_products_pages_localizations_links_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.search_products_pages_localizations_links
    ADD CONSTRAINT search_products_pages_localizations_links_pkey PRIMARY KEY (id);


--
-- TOC entry 3593 (class 2606 OID 214632)
-- Name: search_products_pages_localizations_links search_products_pages_localizations_links_unique; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.search_products_pages_localizations_links
    ADD CONSTRAINT search_products_pages_localizations_links_unique UNIQUE (search_products_page_id, inv_search_products_page_id);


--
-- TOC entry 3578 (class 2606 OID 214602)
-- Name: search_products_pages search_products_pages_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.search_products_pages
    ADD CONSTRAINT search_products_pages_pkey PRIMARY KEY (id);


--
-- TOC entry 3586 (class 2606 OID 214620)
-- Name: search_products_pages_components search_products_pages_unique; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.search_products_pages_components
    ADD CONSTRAINT search_products_pages_unique UNIQUE (entity_id, component_id, field, component_type);


--
-- TOC entry 3316 (class 2606 OID 16687)
-- Name: strapi_api_token_permissions strapi_api_token_permissions_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.strapi_api_token_permissions
    ADD CONSTRAINT strapi_api_token_permissions_pkey PRIMARY KEY (id);


--
-- TOC entry 3322 (class 2606 OID 16689)
-- Name: strapi_api_token_permissions_token_links strapi_api_token_permissions_token_links_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.strapi_api_token_permissions_token_links
    ADD CONSTRAINT strapi_api_token_permissions_token_links_pkey PRIMARY KEY (id);


--
-- TOC entry 3324 (class 2606 OID 16691)
-- Name: strapi_api_token_permissions_token_links strapi_api_token_permissions_token_links_unique; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.strapi_api_token_permissions_token_links
    ADD CONSTRAINT strapi_api_token_permissions_token_links_unique UNIQUE (api_token_permission_id, api_token_id);


--
-- TOC entry 3327 (class 2606 OID 16693)
-- Name: strapi_api_tokens strapi_api_tokens_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.strapi_api_tokens
    ADD CONSTRAINT strapi_api_tokens_pkey PRIMARY KEY (id);


--
-- TOC entry 3330 (class 2606 OID 16695)
-- Name: strapi_core_store_settings strapi_core_store_settings_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.strapi_core_store_settings
    ADD CONSTRAINT strapi_core_store_settings_pkey PRIMARY KEY (id);


--
-- TOC entry 3332 (class 2606 OID 16697)
-- Name: strapi_database_schema strapi_database_schema_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.strapi_database_schema
    ADD CONSTRAINT strapi_database_schema_pkey PRIMARY KEY (id);


--
-- TOC entry 3334 (class 2606 OID 16699)
-- Name: strapi_migrations strapi_migrations_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.strapi_migrations
    ADD CONSTRAINT strapi_migrations_pkey PRIMARY KEY (id);


--
-- TOC entry 3336 (class 2606 OID 16701)
-- Name: strapi_webhooks strapi_webhooks_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.strapi_webhooks
    ADD CONSTRAINT strapi_webhooks_pkey PRIMARY KEY (id);


--
-- TOC entry 3339 (class 2606 OID 16703)
-- Name: up_permissions up_permissions_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.up_permissions
    ADD CONSTRAINT up_permissions_pkey PRIMARY KEY (id);


--
-- TOC entry 3345 (class 2606 OID 16705)
-- Name: up_permissions_role_links up_permissions_role_links_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.up_permissions_role_links
    ADD CONSTRAINT up_permissions_role_links_pkey PRIMARY KEY (id);


--
-- TOC entry 3347 (class 2606 OID 16707)
-- Name: up_permissions_role_links up_permissions_role_links_unique; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.up_permissions_role_links
    ADD CONSTRAINT up_permissions_role_links_unique UNIQUE (permission_id, role_id);


--
-- TOC entry 3350 (class 2606 OID 16709)
-- Name: up_roles up_roles_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.up_roles
    ADD CONSTRAINT up_roles_pkey PRIMARY KEY (id);


--
-- TOC entry 3354 (class 2606 OID 16711)
-- Name: up_users up_users_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.up_users
    ADD CONSTRAINT up_users_pkey PRIMARY KEY (id);


--
-- TOC entry 3360 (class 2606 OID 16713)
-- Name: up_users_role_links up_users_role_links_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.up_users_role_links
    ADD CONSTRAINT up_users_role_links_pkey PRIMARY KEY (id);


--
-- TOC entry 3362 (class 2606 OID 16715)
-- Name: up_users_role_links up_users_role_links_unique; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.up_users_role_links
    ADD CONSTRAINT up_users_role_links_unique UNIQUE (user_id, role_id);


--
-- TOC entry 3375 (class 2606 OID 16717)
-- Name: upload_folders_parent_links upload_folders_parent_links_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.upload_folders_parent_links
    ADD CONSTRAINT upload_folders_parent_links_pkey PRIMARY KEY (id);


--
-- TOC entry 3377 (class 2606 OID 16719)
-- Name: upload_folders_parent_links upload_folders_parent_links_unique; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.upload_folders_parent_links
    ADD CONSTRAINT upload_folders_parent_links_unique UNIQUE (folder_id, inv_folder_id);


--
-- TOC entry 3365 (class 2606 OID 16721)
-- Name: upload_folders upload_folders_path_id_index; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.upload_folders
    ADD CONSTRAINT upload_folders_path_id_index UNIQUE (path_id);


--
-- TOC entry 3367 (class 2606 OID 16723)
-- Name: upload_folders upload_folders_path_index; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.upload_folders
    ADD CONSTRAINT upload_folders_path_index UNIQUE (path);


--
-- TOC entry 3369 (class 2606 OID 16725)
-- Name: upload_folders upload_folders_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.upload_folders
    ADD CONSTRAINT upload_folders_pkey PRIMARY KEY (id);


--
-- TOC entry 3228 (class 1259 OID 16726)
-- Name: admin_permissions_created_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX admin_permissions_created_by_id_fk ON public.admin_permissions USING btree (created_by_id);


--
-- TOC entry 3232 (class 1259 OID 16727)
-- Name: admin_permissions_role_links_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX admin_permissions_role_links_fk ON public.admin_permissions_role_links USING btree (permission_id);


--
-- TOC entry 3233 (class 1259 OID 16728)
-- Name: admin_permissions_role_links_inv_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX admin_permissions_role_links_inv_fk ON public.admin_permissions_role_links USING btree (role_id);


--
-- TOC entry 3234 (class 1259 OID 16729)
-- Name: admin_permissions_role_links_order_inv_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX admin_permissions_role_links_order_inv_fk ON public.admin_permissions_role_links USING btree (permission_order);


--
-- TOC entry 3231 (class 1259 OID 16730)
-- Name: admin_permissions_updated_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX admin_permissions_updated_by_id_fk ON public.admin_permissions USING btree (updated_by_id);


--
-- TOC entry 3239 (class 1259 OID 16731)
-- Name: admin_roles_created_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX admin_roles_created_by_id_fk ON public.admin_roles USING btree (created_by_id);


--
-- TOC entry 3242 (class 1259 OID 16732)
-- Name: admin_roles_updated_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX admin_roles_updated_by_id_fk ON public.admin_roles USING btree (updated_by_id);


--
-- TOC entry 3243 (class 1259 OID 16733)
-- Name: admin_users_created_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX admin_users_created_by_id_fk ON public.admin_users USING btree (created_by_id);


--
-- TOC entry 3247 (class 1259 OID 16734)
-- Name: admin_users_roles_links_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX admin_users_roles_links_fk ON public.admin_users_roles_links USING btree (user_id);


--
-- TOC entry 3248 (class 1259 OID 16735)
-- Name: admin_users_roles_links_inv_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX admin_users_roles_links_inv_fk ON public.admin_users_roles_links USING btree (role_id);


--
-- TOC entry 3249 (class 1259 OID 16736)
-- Name: admin_users_roles_links_order_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX admin_users_roles_links_order_fk ON public.admin_users_roles_links USING btree (role_order);


--
-- TOC entry 3250 (class 1259 OID 16737)
-- Name: admin_users_roles_links_order_inv_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX admin_users_roles_links_order_inv_fk ON public.admin_users_roles_links USING btree (user_order);


--
-- TOC entry 3246 (class 1259 OID 16738)
-- Name: admin_users_updated_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX admin_users_updated_by_id_fk ON public.admin_users USING btree (updated_by_id);


--
-- TOC entry 3490 (class 1259 OID 214115)
-- Name: available_products_pages_component_type_index; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX available_products_pages_component_type_index ON public.available_products_pages_components USING btree (component_type);


--
-- TOC entry 3486 (class 1259 OID 214101)
-- Name: available_products_pages_created_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX available_products_pages_created_by_id_fk ON public.available_products_pages USING btree (created_by_id);


--
-- TOC entry 3493 (class 1259 OID 214116)
-- Name: available_products_pages_entity_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX available_products_pages_entity_fk ON public.available_products_pages_components USING btree (entity_id);


--
-- TOC entry 3494 (class 1259 OID 214114)
-- Name: available_products_pages_field_index; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX available_products_pages_field_index ON public.available_products_pages_components USING btree (field);


--
-- TOC entry 3497 (class 1259 OID 214127)
-- Name: available_products_pages_localizations_links_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX available_products_pages_localizations_links_fk ON public.available_products_pages_localizations_links USING btree (available_products_page_id);


--
-- TOC entry 3498 (class 1259 OID 214128)
-- Name: available_products_pages_localizations_links_inv_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX available_products_pages_localizations_links_inv_fk ON public.available_products_pages_localizations_links USING btree (inv_available_products_page_id);


--
-- TOC entry 3499 (class 1259 OID 214131)
-- Name: available_products_pages_localizations_links_order_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX available_products_pages_localizations_links_order_fk ON public.available_products_pages_localizations_links USING btree (available_products_page_order);


--
-- TOC entry 3489 (class 1259 OID 214102)
-- Name: available_products_pages_updated_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX available_products_pages_updated_by_id_fk ON public.available_products_pages USING btree (updated_by_id);


--
-- TOC entry 3544 (class 1259 OID 214434)
-- Name: basket_pages_component_type_index; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX basket_pages_component_type_index ON public.basket_pages_components USING btree (component_type);


--
-- TOC entry 3540 (class 1259 OID 214420)
-- Name: basket_pages_created_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX basket_pages_created_by_id_fk ON public.basket_pages USING btree (created_by_id);


--
-- TOC entry 3547 (class 1259 OID 214435)
-- Name: basket_pages_entity_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX basket_pages_entity_fk ON public.basket_pages_components USING btree (entity_id);


--
-- TOC entry 3548 (class 1259 OID 214433)
-- Name: basket_pages_field_index; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX basket_pages_field_index ON public.basket_pages_components USING btree (field);


--
-- TOC entry 3551 (class 1259 OID 214446)
-- Name: basket_pages_localizations_links_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX basket_pages_localizations_links_fk ON public.basket_pages_localizations_links USING btree (basket_page_id);


--
-- TOC entry 3552 (class 1259 OID 214447)
-- Name: basket_pages_localizations_links_inv_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX basket_pages_localizations_links_inv_fk ON public.basket_pages_localizations_links USING btree (inv_basket_page_id);


--
-- TOC entry 3553 (class 1259 OID 214450)
-- Name: basket_pages_localizations_links_order_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX basket_pages_localizations_links_order_fk ON public.basket_pages_localizations_links USING btree (basket_page_order);


--
-- TOC entry 3543 (class 1259 OID 214421)
-- Name: basket_pages_updated_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX basket_pages_updated_by_id_fk ON public.basket_pages USING btree (updated_by_id);


--
-- TOC entry 3257 (class 1259 OID 16739)
-- Name: components_blocks_hero_slider_items_component_type_index; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX components_blocks_hero_slider_items_component_type_index ON public.components_blocks_hero_slider_items_components USING btree (component_type);


--
-- TOC entry 3260 (class 1259 OID 16740)
-- Name: components_blocks_hero_slider_items_entity_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX components_blocks_hero_slider_items_entity_fk ON public.components_blocks_hero_slider_items_components USING btree (entity_id);


--
-- TOC entry 3261 (class 1259 OID 16741)
-- Name: components_blocks_hero_slider_items_field_index; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX components_blocks_hero_slider_items_field_index ON public.components_blocks_hero_slider_items_components USING btree (field);


--
-- TOC entry 3266 (class 1259 OID 16742)
-- Name: components_blocks_hero_sliders_component_type_index; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX components_blocks_hero_sliders_component_type_index ON public.components_blocks_hero_sliders_components USING btree (component_type);


--
-- TOC entry 3269 (class 1259 OID 16743)
-- Name: components_blocks_hero_sliders_entity_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX components_blocks_hero_sliders_entity_fk ON public.components_blocks_hero_sliders_components USING btree (entity_id);


--
-- TOC entry 3270 (class 1259 OID 16744)
-- Name: components_blocks_hero_sliders_field_index; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX components_blocks_hero_sliders_field_index ON public.components_blocks_hero_sliders_components USING btree (field);


--
-- TOC entry 3402 (class 1259 OID 213698)
-- Name: dashboard_pages_component_type_index; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX dashboard_pages_component_type_index ON public.dashboard_pages_components USING btree (component_type);


--
-- TOC entry 3378 (class 1259 OID 213624)
-- Name: dashboard_pages_created_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX dashboard_pages_created_by_id_fk ON public.dashboard_pages USING btree (created_by_id);


--
-- TOC entry 3405 (class 1259 OID 213699)
-- Name: dashboard_pages_entity_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX dashboard_pages_entity_fk ON public.dashboard_pages_components USING btree (entity_id);


--
-- TOC entry 3406 (class 1259 OID 213697)
-- Name: dashboard_pages_field_index; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX dashboard_pages_field_index ON public.dashboard_pages_components USING btree (field);


--
-- TOC entry 3409 (class 1259 OID 213710)
-- Name: dashboard_pages_localizations_links_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX dashboard_pages_localizations_links_fk ON public.dashboard_pages_localizations_links USING btree (dashboard_page_id);


--
-- TOC entry 3410 (class 1259 OID 213711)
-- Name: dashboard_pages_localizations_links_inv_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX dashboard_pages_localizations_links_inv_fk ON public.dashboard_pages_localizations_links USING btree (inv_dashboard_page_id);


--
-- TOC entry 3411 (class 1259 OID 213714)
-- Name: dashboard_pages_localizations_links_order_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX dashboard_pages_localizations_links_order_fk ON public.dashboard_pages_localizations_links USING btree (dashboard_page_order);


--
-- TOC entry 3381 (class 1259 OID 213625)
-- Name: dashboard_pages_updated_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX dashboard_pages_updated_by_id_fk ON public.dashboard_pages USING btree (updated_by_id);


--
-- TOC entry 3416 (class 1259 OID 213727)
-- Name: download_center_pages_component_type_index; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX download_center_pages_component_type_index ON public.download_center_pages_components USING btree (component_type);


--
-- TOC entry 3382 (class 1259 OID 213634)
-- Name: download_center_pages_created_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX download_center_pages_created_by_id_fk ON public.download_center_pages USING btree (created_by_id);


--
-- TOC entry 3419 (class 1259 OID 213728)
-- Name: download_center_pages_entity_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX download_center_pages_entity_fk ON public.download_center_pages_components USING btree (entity_id);


--
-- TOC entry 3420 (class 1259 OID 213726)
-- Name: download_center_pages_field_index; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX download_center_pages_field_index ON public.download_center_pages_components USING btree (field);


--
-- TOC entry 3423 (class 1259 OID 213739)
-- Name: download_center_pages_localizations_links_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX download_center_pages_localizations_links_fk ON public.download_center_pages_localizations_links USING btree (download_center_page_id);


--
-- TOC entry 3424 (class 1259 OID 213740)
-- Name: download_center_pages_localizations_links_inv_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX download_center_pages_localizations_links_inv_fk ON public.download_center_pages_localizations_links USING btree (inv_download_center_page_id);


--
-- TOC entry 3425 (class 1259 OID 213743)
-- Name: download_center_pages_localizations_links_order_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX download_center_pages_localizations_links_order_fk ON public.download_center_pages_localizations_links USING btree (download_center_page_order);


--
-- TOC entry 3385 (class 1259 OID 213635)
-- Name: download_center_pages_updated_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX download_center_pages_updated_by_id_fk ON public.download_center_pages USING btree (updated_by_id);


--
-- TOC entry 3277 (class 1259 OID 16745)
-- Name: files_created_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX files_created_by_id_fk ON public.files USING btree (created_by_id);


--
-- TOC entry 3282 (class 1259 OID 16746)
-- Name: files_folder_links_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX files_folder_links_fk ON public.files_folder_links USING btree (file_id);


--
-- TOC entry 3283 (class 1259 OID 16747)
-- Name: files_folder_links_inv_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX files_folder_links_inv_fk ON public.files_folder_links USING btree (folder_id);


--
-- TOC entry 3284 (class 1259 OID 16748)
-- Name: files_folder_links_order_inv_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX files_folder_links_order_inv_fk ON public.files_folder_links USING btree (file_order);


--
-- TOC entry 3289 (class 1259 OID 16749)
-- Name: files_related_morphs_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX files_related_morphs_fk ON public.files_related_morphs USING btree (file_id);


--
-- TOC entry 3280 (class 1259 OID 16750)
-- Name: files_updated_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX files_updated_by_id_fk ON public.files USING btree (updated_by_id);


--
-- TOC entry 3296 (class 1259 OID 16751)
-- Name: home_pages_component_type_index; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX home_pages_component_type_index ON public.home_pages_components USING btree (component_type);


--
-- TOC entry 3292 (class 1259 OID 16752)
-- Name: home_pages_created_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX home_pages_created_by_id_fk ON public.home_pages USING btree (created_by_id);


--
-- TOC entry 3299 (class 1259 OID 16753)
-- Name: home_pages_entity_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX home_pages_entity_fk ON public.home_pages_components USING btree (entity_id);


--
-- TOC entry 3300 (class 1259 OID 16754)
-- Name: home_pages_field_index; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX home_pages_field_index ON public.home_pages_components USING btree (field);


--
-- TOC entry 3303 (class 1259 OID 16755)
-- Name: home_pages_localizations_links_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX home_pages_localizations_links_fk ON public.home_pages_localizations_links USING btree (home_page_id);


--
-- TOC entry 3304 (class 1259 OID 16756)
-- Name: home_pages_localizations_links_inv_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX home_pages_localizations_links_inv_fk ON public.home_pages_localizations_links USING btree (inv_home_page_id);


--
-- TOC entry 3305 (class 1259 OID 16757)
-- Name: home_pages_localizations_links_order_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX home_pages_localizations_links_order_fk ON public.home_pages_localizations_links USING btree (home_page_order);


--
-- TOC entry 3295 (class 1259 OID 16758)
-- Name: home_pages_updated_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX home_pages_updated_by_id_fk ON public.home_pages USING btree (updated_by_id);


--
-- TOC entry 3310 (class 1259 OID 16759)
-- Name: i18n_locale_created_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX i18n_locale_created_by_id_fk ON public.i18n_locale USING btree (created_by_id);


--
-- TOC entry 3313 (class 1259 OID 16760)
-- Name: i18n_locale_updated_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX i18n_locale_updated_by_id_fk ON public.i18n_locale USING btree (updated_by_id);


--
-- TOC entry 3430 (class 1259 OID 213785)
-- Name: news_pages_component_type_index; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX news_pages_component_type_index ON public.news_pages_components USING btree (component_type);


--
-- TOC entry 3386 (class 1259 OID 213654)
-- Name: news_pages_created_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX news_pages_created_by_id_fk ON public.news_pages USING btree (created_by_id);


--
-- TOC entry 3433 (class 1259 OID 213786)
-- Name: news_pages_entity_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX news_pages_entity_fk ON public.news_pages_components USING btree (entity_id);


--
-- TOC entry 3434 (class 1259 OID 213784)
-- Name: news_pages_field_index; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX news_pages_field_index ON public.news_pages_components USING btree (field);


--
-- TOC entry 3437 (class 1259 OID 213797)
-- Name: news_pages_localizations_links_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX news_pages_localizations_links_fk ON public.news_pages_localizations_links USING btree (news_page_id);


--
-- TOC entry 3438 (class 1259 OID 213798)
-- Name: news_pages_localizations_links_inv_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX news_pages_localizations_links_inv_fk ON public.news_pages_localizations_links USING btree (inv_news_page_id);


--
-- TOC entry 3439 (class 1259 OID 213801)
-- Name: news_pages_localizations_links_order_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX news_pages_localizations_links_order_fk ON public.news_pages_localizations_links USING btree (news_page_order);


--
-- TOC entry 3389 (class 1259 OID 213655)
-- Name: news_pages_updated_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX news_pages_updated_by_id_fk ON public.news_pages USING btree (updated_by_id);


--
-- TOC entry 3562 (class 1259 OID 214525)
-- Name: order_item_pages_component_type_index; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX order_item_pages_component_type_index ON public.order_item_pages_components USING btree (component_type);


--
-- TOC entry 3558 (class 1259 OID 214511)
-- Name: order_item_pages_created_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX order_item_pages_created_by_id_fk ON public.order_item_pages USING btree (created_by_id);


--
-- TOC entry 3565 (class 1259 OID 214526)
-- Name: order_item_pages_entity_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX order_item_pages_entity_fk ON public.order_item_pages_components USING btree (entity_id);


--
-- TOC entry 3566 (class 1259 OID 214524)
-- Name: order_item_pages_field_index; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX order_item_pages_field_index ON public.order_item_pages_components USING btree (field);


--
-- TOC entry 3569 (class 1259 OID 214537)
-- Name: order_item_pages_localizations_links_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX order_item_pages_localizations_links_fk ON public.order_item_pages_localizations_links USING btree (order_item_page_id);


--
-- TOC entry 3570 (class 1259 OID 214538)
-- Name: order_item_pages_localizations_links_inv_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX order_item_pages_localizations_links_inv_fk ON public.order_item_pages_localizations_links USING btree (inv_order_item_page_id);


--
-- TOC entry 3571 (class 1259 OID 214541)
-- Name: order_item_pages_localizations_links_order_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX order_item_pages_localizations_links_order_fk ON public.order_item_pages_localizations_links USING btree (order_item_page_order);


--
-- TOC entry 3561 (class 1259 OID 214512)
-- Name: order_item_pages_updated_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX order_item_pages_updated_by_id_fk ON public.order_item_pages USING btree (updated_by_id);


--
-- TOC entry 3444 (class 1259 OID 213814)
-- Name: order_pages_component_type_index; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX order_pages_component_type_index ON public.order_pages_components USING btree (component_type);


--
-- TOC entry 3390 (class 1259 OID 213664)
-- Name: order_pages_created_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX order_pages_created_by_id_fk ON public.order_pages USING btree (created_by_id);


--
-- TOC entry 3447 (class 1259 OID 213815)
-- Name: order_pages_entity_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX order_pages_entity_fk ON public.order_pages_components USING btree (entity_id);


--
-- TOC entry 3448 (class 1259 OID 213813)
-- Name: order_pages_field_index; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX order_pages_field_index ON public.order_pages_components USING btree (field);


--
-- TOC entry 3451 (class 1259 OID 213826)
-- Name: order_pages_localizations_links_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX order_pages_localizations_links_fk ON public.order_pages_localizations_links USING btree (order_page_id);


--
-- TOC entry 3452 (class 1259 OID 213827)
-- Name: order_pages_localizations_links_inv_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX order_pages_localizations_links_inv_fk ON public.order_pages_localizations_links USING btree (inv_order_page_id);


--
-- TOC entry 3453 (class 1259 OID 213830)
-- Name: order_pages_localizations_links_order_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX order_pages_localizations_links_order_fk ON public.order_pages_localizations_links USING btree (order_page_order);


--
-- TOC entry 3393 (class 1259 OID 213665)
-- Name: order_pages_updated_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX order_pages_updated_by_id_fk ON public.order_pages USING btree (updated_by_id);


--
-- TOC entry 3458 (class 1259 OID 213843)
-- Name: orders_pages_component_type_index; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX orders_pages_component_type_index ON public.orders_pages_components USING btree (component_type);


--
-- TOC entry 3394 (class 1259 OID 213674)
-- Name: orders_pages_created_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX orders_pages_created_by_id_fk ON public.orders_pages USING btree (created_by_id);


--
-- TOC entry 3461 (class 1259 OID 213844)
-- Name: orders_pages_entity_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX orders_pages_entity_fk ON public.orders_pages_components USING btree (entity_id);


--
-- TOC entry 3462 (class 1259 OID 213842)
-- Name: orders_pages_field_index; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX orders_pages_field_index ON public.orders_pages_components USING btree (field);


--
-- TOC entry 3465 (class 1259 OID 213855)
-- Name: orders_pages_localizations_links_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX orders_pages_localizations_links_fk ON public.orders_pages_localizations_links USING btree (orders_page_id);


--
-- TOC entry 3466 (class 1259 OID 213856)
-- Name: orders_pages_localizations_links_inv_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX orders_pages_localizations_links_inv_fk ON public.orders_pages_localizations_links USING btree (inv_orders_page_id);


--
-- TOC entry 3467 (class 1259 OID 213859)
-- Name: orders_pages_localizations_links_order_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX orders_pages_localizations_links_order_fk ON public.orders_pages_localizations_links USING btree (orders_page_order);


--
-- TOC entry 3397 (class 1259 OID 213675)
-- Name: orders_pages_updated_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX orders_pages_updated_by_id_fk ON public.orders_pages USING btree (updated_by_id);


--
-- TOC entry 3472 (class 1259 OID 213872)
-- Name: outlet_pages_component_type_index; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX outlet_pages_component_type_index ON public.outlet_pages_components USING btree (component_type);


--
-- TOC entry 3398 (class 1259 OID 213684)
-- Name: outlet_pages_created_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX outlet_pages_created_by_id_fk ON public.outlet_pages USING btree (created_by_id);


--
-- TOC entry 3475 (class 1259 OID 213873)
-- Name: outlet_pages_entity_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX outlet_pages_entity_fk ON public.outlet_pages_components USING btree (entity_id);


--
-- TOC entry 3476 (class 1259 OID 213871)
-- Name: outlet_pages_field_index; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX outlet_pages_field_index ON public.outlet_pages_components USING btree (field);


--
-- TOC entry 3479 (class 1259 OID 213884)
-- Name: outlet_pages_localizations_links_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX outlet_pages_localizations_links_fk ON public.outlet_pages_localizations_links USING btree (outlet_page_id);


--
-- TOC entry 3480 (class 1259 OID 213885)
-- Name: outlet_pages_localizations_links_inv_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX outlet_pages_localizations_links_inv_fk ON public.outlet_pages_localizations_links USING btree (inv_outlet_page_id);


--
-- TOC entry 3481 (class 1259 OID 213888)
-- Name: outlet_pages_localizations_links_order_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX outlet_pages_localizations_links_order_fk ON public.outlet_pages_localizations_links USING btree (outlet_page_order);


--
-- TOC entry 3401 (class 1259 OID 213685)
-- Name: outlet_pages_updated_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX outlet_pages_updated_by_id_fk ON public.outlet_pages USING btree (updated_by_id);


--
-- TOC entry 3526 (class 1259 OID 214294)
-- Name: privacy_policy_pages_component_type_index; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX privacy_policy_pages_component_type_index ON public.privacy_policy_pages_components USING btree (component_type);


--
-- TOC entry 3522 (class 1259 OID 214280)
-- Name: privacy_policy_pages_created_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX privacy_policy_pages_created_by_id_fk ON public.privacy_policy_pages USING btree (created_by_id);


--
-- TOC entry 3529 (class 1259 OID 214295)
-- Name: privacy_policy_pages_entity_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX privacy_policy_pages_entity_fk ON public.privacy_policy_pages_components USING btree (entity_id);


--
-- TOC entry 3530 (class 1259 OID 214293)
-- Name: privacy_policy_pages_field_index; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX privacy_policy_pages_field_index ON public.privacy_policy_pages_components USING btree (field);


--
-- TOC entry 3533 (class 1259 OID 214306)
-- Name: privacy_policy_pages_localizations_links_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX privacy_policy_pages_localizations_links_fk ON public.privacy_policy_pages_localizations_links USING btree (privacy_policy_page_id);


--
-- TOC entry 3534 (class 1259 OID 214307)
-- Name: privacy_policy_pages_localizations_links_inv_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX privacy_policy_pages_localizations_links_inv_fk ON public.privacy_policy_pages_localizations_links USING btree (inv_privacy_policy_page_id);


--
-- TOC entry 3535 (class 1259 OID 214310)
-- Name: privacy_policy_pages_localizations_links_order_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX privacy_policy_pages_localizations_links_order_fk ON public.privacy_policy_pages_localizations_links USING btree (privacy_policy_page_order);


--
-- TOC entry 3525 (class 1259 OID 214281)
-- Name: privacy_policy_pages_updated_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX privacy_policy_pages_updated_by_id_fk ON public.privacy_policy_pages USING btree (updated_by_id);


--
-- TOC entry 3508 (class 1259 OID 214204)
-- Name: regulations_pages_component_type_index; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX regulations_pages_component_type_index ON public.regulations_pages_components USING btree (component_type);


--
-- TOC entry 3504 (class 1259 OID 214190)
-- Name: regulations_pages_created_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX regulations_pages_created_by_id_fk ON public.regulations_pages USING btree (created_by_id);


--
-- TOC entry 3511 (class 1259 OID 214205)
-- Name: regulations_pages_entity_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX regulations_pages_entity_fk ON public.regulations_pages_components USING btree (entity_id);


--
-- TOC entry 3512 (class 1259 OID 214203)
-- Name: regulations_pages_field_index; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX regulations_pages_field_index ON public.regulations_pages_components USING btree (field);


--
-- TOC entry 3515 (class 1259 OID 214216)
-- Name: regulations_pages_localizations_links_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX regulations_pages_localizations_links_fk ON public.regulations_pages_localizations_links USING btree (regulations_page_id);


--
-- TOC entry 3516 (class 1259 OID 214217)
-- Name: regulations_pages_localizations_links_inv_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX regulations_pages_localizations_links_inv_fk ON public.regulations_pages_localizations_links USING btree (inv_regulations_page_id);


--
-- TOC entry 3517 (class 1259 OID 214220)
-- Name: regulations_pages_localizations_links_order_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX regulations_pages_localizations_links_order_fk ON public.regulations_pages_localizations_links USING btree (regulations_page_order);


--
-- TOC entry 3507 (class 1259 OID 214191)
-- Name: regulations_pages_updated_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX regulations_pages_updated_by_id_fk ON public.regulations_pages USING btree (updated_by_id);


--
-- TOC entry 3580 (class 1259 OID 214617)
-- Name: search_products_pages_component_type_index; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX search_products_pages_component_type_index ON public.search_products_pages_components USING btree (component_type);


--
-- TOC entry 3576 (class 1259 OID 214603)
-- Name: search_products_pages_created_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX search_products_pages_created_by_id_fk ON public.search_products_pages USING btree (created_by_id);


--
-- TOC entry 3583 (class 1259 OID 214618)
-- Name: search_products_pages_entity_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX search_products_pages_entity_fk ON public.search_products_pages_components USING btree (entity_id);


--
-- TOC entry 3584 (class 1259 OID 214616)
-- Name: search_products_pages_field_index; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX search_products_pages_field_index ON public.search_products_pages_components USING btree (field);


--
-- TOC entry 3587 (class 1259 OID 214629)
-- Name: search_products_pages_localizations_links_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX search_products_pages_localizations_links_fk ON public.search_products_pages_localizations_links USING btree (search_products_page_id);


--
-- TOC entry 3588 (class 1259 OID 214630)
-- Name: search_products_pages_localizations_links_inv_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX search_products_pages_localizations_links_inv_fk ON public.search_products_pages_localizations_links USING btree (inv_search_products_page_id);


--
-- TOC entry 3589 (class 1259 OID 214633)
-- Name: search_products_pages_localizations_links_order_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX search_products_pages_localizations_links_order_fk ON public.search_products_pages_localizations_links USING btree (search_products_page_order);


--
-- TOC entry 3579 (class 1259 OID 214604)
-- Name: search_products_pages_updated_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX search_products_pages_updated_by_id_fk ON public.search_products_pages USING btree (updated_by_id);


--
-- TOC entry 3314 (class 1259 OID 16761)
-- Name: strapi_api_token_permissions_created_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX strapi_api_token_permissions_created_by_id_fk ON public.strapi_api_token_permissions USING btree (created_by_id);


--
-- TOC entry 3318 (class 1259 OID 16762)
-- Name: strapi_api_token_permissions_token_links_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX strapi_api_token_permissions_token_links_fk ON public.strapi_api_token_permissions_token_links USING btree (api_token_permission_id);


--
-- TOC entry 3319 (class 1259 OID 16763)
-- Name: strapi_api_token_permissions_token_links_inv_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX strapi_api_token_permissions_token_links_inv_fk ON public.strapi_api_token_permissions_token_links USING btree (api_token_id);


--
-- TOC entry 3320 (class 1259 OID 16764)
-- Name: strapi_api_token_permissions_token_links_order_inv_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX strapi_api_token_permissions_token_links_order_inv_fk ON public.strapi_api_token_permissions_token_links USING btree (api_token_permission_order);


--
-- TOC entry 3317 (class 1259 OID 16765)
-- Name: strapi_api_token_permissions_updated_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX strapi_api_token_permissions_updated_by_id_fk ON public.strapi_api_token_permissions USING btree (updated_by_id);


--
-- TOC entry 3325 (class 1259 OID 16766)
-- Name: strapi_api_tokens_created_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX strapi_api_tokens_created_by_id_fk ON public.strapi_api_tokens USING btree (created_by_id);


--
-- TOC entry 3328 (class 1259 OID 16767)
-- Name: strapi_api_tokens_updated_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX strapi_api_tokens_updated_by_id_fk ON public.strapi_api_tokens USING btree (updated_by_id);


--
-- TOC entry 3337 (class 1259 OID 16768)
-- Name: up_permissions_created_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX up_permissions_created_by_id_fk ON public.up_permissions USING btree (created_by_id);


--
-- TOC entry 3341 (class 1259 OID 16769)
-- Name: up_permissions_role_links_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX up_permissions_role_links_fk ON public.up_permissions_role_links USING btree (permission_id);


--
-- TOC entry 3342 (class 1259 OID 16770)
-- Name: up_permissions_role_links_inv_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX up_permissions_role_links_inv_fk ON public.up_permissions_role_links USING btree (role_id);


--
-- TOC entry 3343 (class 1259 OID 16771)
-- Name: up_permissions_role_links_order_inv_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX up_permissions_role_links_order_inv_fk ON public.up_permissions_role_links USING btree (permission_order);


--
-- TOC entry 3340 (class 1259 OID 16772)
-- Name: up_permissions_updated_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX up_permissions_updated_by_id_fk ON public.up_permissions USING btree (updated_by_id);


--
-- TOC entry 3348 (class 1259 OID 16773)
-- Name: up_roles_created_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX up_roles_created_by_id_fk ON public.up_roles USING btree (created_by_id);


--
-- TOC entry 3351 (class 1259 OID 16774)
-- Name: up_roles_updated_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX up_roles_updated_by_id_fk ON public.up_roles USING btree (updated_by_id);


--
-- TOC entry 3352 (class 1259 OID 16775)
-- Name: up_users_created_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX up_users_created_by_id_fk ON public.up_users USING btree (created_by_id);


--
-- TOC entry 3356 (class 1259 OID 16776)
-- Name: up_users_role_links_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX up_users_role_links_fk ON public.up_users_role_links USING btree (user_id);


--
-- TOC entry 3357 (class 1259 OID 16777)
-- Name: up_users_role_links_inv_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX up_users_role_links_inv_fk ON public.up_users_role_links USING btree (role_id);


--
-- TOC entry 3358 (class 1259 OID 16778)
-- Name: up_users_role_links_order_inv_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX up_users_role_links_order_inv_fk ON public.up_users_role_links USING btree (user_order);


--
-- TOC entry 3355 (class 1259 OID 16779)
-- Name: up_users_updated_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX up_users_updated_by_id_fk ON public.up_users USING btree (updated_by_id);


--
-- TOC entry 3281 (class 1259 OID 16780)
-- Name: upload_files_folder_path_index; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX upload_files_folder_path_index ON public.files USING btree (folder_path);


--
-- TOC entry 3363 (class 1259 OID 16781)
-- Name: upload_folders_created_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX upload_folders_created_by_id_fk ON public.upload_folders USING btree (created_by_id);


--
-- TOC entry 3371 (class 1259 OID 16782)
-- Name: upload_folders_parent_links_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX upload_folders_parent_links_fk ON public.upload_folders_parent_links USING btree (folder_id);


--
-- TOC entry 3372 (class 1259 OID 16783)
-- Name: upload_folders_parent_links_inv_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX upload_folders_parent_links_inv_fk ON public.upload_folders_parent_links USING btree (inv_folder_id);


--
-- TOC entry 3373 (class 1259 OID 16784)
-- Name: upload_folders_parent_links_order_inv_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX upload_folders_parent_links_order_inv_fk ON public.upload_folders_parent_links USING btree (folder_order);


--
-- TOC entry 3370 (class 1259 OID 16785)
-- Name: upload_folders_updated_by_id_fk; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX upload_folders_updated_by_id_fk ON public.upload_folders USING btree (updated_by_id);


--
-- TOC entry 3594 (class 2606 OID 16786)
-- Name: admin_permissions admin_permissions_created_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.admin_permissions
    ADD CONSTRAINT admin_permissions_created_by_id_fk FOREIGN KEY (created_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3596 (class 2606 OID 16791)
-- Name: admin_permissions_role_links admin_permissions_role_links_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.admin_permissions_role_links
    ADD CONSTRAINT admin_permissions_role_links_fk FOREIGN KEY (permission_id) REFERENCES public.admin_permissions(id) ON DELETE CASCADE;


--
-- TOC entry 3597 (class 2606 OID 16796)
-- Name: admin_permissions_role_links admin_permissions_role_links_inv_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.admin_permissions_role_links
    ADD CONSTRAINT admin_permissions_role_links_inv_fk FOREIGN KEY (role_id) REFERENCES public.admin_roles(id) ON DELETE CASCADE;


--
-- TOC entry 3595 (class 2606 OID 16801)
-- Name: admin_permissions admin_permissions_updated_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.admin_permissions
    ADD CONSTRAINT admin_permissions_updated_by_id_fk FOREIGN KEY (updated_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3598 (class 2606 OID 16806)
-- Name: admin_roles admin_roles_created_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.admin_roles
    ADD CONSTRAINT admin_roles_created_by_id_fk FOREIGN KEY (created_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3599 (class 2606 OID 16811)
-- Name: admin_roles admin_roles_updated_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.admin_roles
    ADD CONSTRAINT admin_roles_updated_by_id_fk FOREIGN KEY (updated_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3600 (class 2606 OID 16816)
-- Name: admin_users admin_users_created_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.admin_users
    ADD CONSTRAINT admin_users_created_by_id_fk FOREIGN KEY (created_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3602 (class 2606 OID 16821)
-- Name: admin_users_roles_links admin_users_roles_links_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.admin_users_roles_links
    ADD CONSTRAINT admin_users_roles_links_fk FOREIGN KEY (user_id) REFERENCES public.admin_users(id) ON DELETE CASCADE;


--
-- TOC entry 3603 (class 2606 OID 16826)
-- Name: admin_users_roles_links admin_users_roles_links_inv_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.admin_users_roles_links
    ADD CONSTRAINT admin_users_roles_links_inv_fk FOREIGN KEY (role_id) REFERENCES public.admin_roles(id) ON DELETE CASCADE;


--
-- TOC entry 3601 (class 2606 OID 16831)
-- Name: admin_users admin_users_updated_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.admin_users
    ADD CONSTRAINT admin_users_updated_by_id_fk FOREIGN KEY (updated_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3668 (class 2606 OID 214132)
-- Name: available_products_pages available_products_pages_created_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.available_products_pages
    ADD CONSTRAINT available_products_pages_created_by_id_fk FOREIGN KEY (created_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3670 (class 2606 OID 214142)
-- Name: available_products_pages_components available_products_pages_entity_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.available_products_pages_components
    ADD CONSTRAINT available_products_pages_entity_fk FOREIGN KEY (entity_id) REFERENCES public.available_products_pages(id) ON DELETE CASCADE;


--
-- TOC entry 3671 (class 2606 OID 214147)
-- Name: available_products_pages_localizations_links available_products_pages_localizations_links_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.available_products_pages_localizations_links
    ADD CONSTRAINT available_products_pages_localizations_links_fk FOREIGN KEY (available_products_page_id) REFERENCES public.available_products_pages(id) ON DELETE CASCADE;


--
-- TOC entry 3672 (class 2606 OID 214152)
-- Name: available_products_pages_localizations_links available_products_pages_localizations_links_inv_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.available_products_pages_localizations_links
    ADD CONSTRAINT available_products_pages_localizations_links_inv_fk FOREIGN KEY (inv_available_products_page_id) REFERENCES public.available_products_pages(id) ON DELETE CASCADE;


--
-- TOC entry 3669 (class 2606 OID 214137)
-- Name: available_products_pages available_products_pages_updated_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.available_products_pages
    ADD CONSTRAINT available_products_pages_updated_by_id_fk FOREIGN KEY (updated_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3683 (class 2606 OID 214451)
-- Name: basket_pages basket_pages_created_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.basket_pages
    ADD CONSTRAINT basket_pages_created_by_id_fk FOREIGN KEY (created_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3685 (class 2606 OID 214461)
-- Name: basket_pages_components basket_pages_entity_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.basket_pages_components
    ADD CONSTRAINT basket_pages_entity_fk FOREIGN KEY (entity_id) REFERENCES public.basket_pages(id) ON DELETE CASCADE;


--
-- TOC entry 3686 (class 2606 OID 214466)
-- Name: basket_pages_localizations_links basket_pages_localizations_links_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.basket_pages_localizations_links
    ADD CONSTRAINT basket_pages_localizations_links_fk FOREIGN KEY (basket_page_id) REFERENCES public.basket_pages(id) ON DELETE CASCADE;


--
-- TOC entry 3687 (class 2606 OID 214471)
-- Name: basket_pages_localizations_links basket_pages_localizations_links_inv_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.basket_pages_localizations_links
    ADD CONSTRAINT basket_pages_localizations_links_inv_fk FOREIGN KEY (inv_basket_page_id) REFERENCES public.basket_pages(id) ON DELETE CASCADE;


--
-- TOC entry 3684 (class 2606 OID 214456)
-- Name: basket_pages basket_pages_updated_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.basket_pages
    ADD CONSTRAINT basket_pages_updated_by_id_fk FOREIGN KEY (updated_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3604 (class 2606 OID 16836)
-- Name: components_blocks_hero_slider_items_components components_blocks_hero_slider_items_entity_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.components_blocks_hero_slider_items_components
    ADD CONSTRAINT components_blocks_hero_slider_items_entity_fk FOREIGN KEY (entity_id) REFERENCES public.components_blocks_hero_slider_items(id) ON DELETE CASCADE;


--
-- TOC entry 3605 (class 2606 OID 16841)
-- Name: components_blocks_hero_sliders_components components_blocks_hero_sliders_entity_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.components_blocks_hero_sliders_components
    ADD CONSTRAINT components_blocks_hero_sliders_entity_fk FOREIGN KEY (entity_id) REFERENCES public.components_blocks_hero_sliders(id) ON DELETE CASCADE;


--
-- TOC entry 3638 (class 2606 OID 213889)
-- Name: dashboard_pages dashboard_pages_created_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.dashboard_pages
    ADD CONSTRAINT dashboard_pages_created_by_id_fk FOREIGN KEY (created_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3650 (class 2606 OID 213959)
-- Name: dashboard_pages_components dashboard_pages_entity_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.dashboard_pages_components
    ADD CONSTRAINT dashboard_pages_entity_fk FOREIGN KEY (entity_id) REFERENCES public.dashboard_pages(id) ON DELETE CASCADE;


--
-- TOC entry 3651 (class 2606 OID 213964)
-- Name: dashboard_pages_localizations_links dashboard_pages_localizations_links_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.dashboard_pages_localizations_links
    ADD CONSTRAINT dashboard_pages_localizations_links_fk FOREIGN KEY (dashboard_page_id) REFERENCES public.dashboard_pages(id) ON DELETE CASCADE;


--
-- TOC entry 3652 (class 2606 OID 213969)
-- Name: dashboard_pages_localizations_links dashboard_pages_localizations_links_inv_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.dashboard_pages_localizations_links
    ADD CONSTRAINT dashboard_pages_localizations_links_inv_fk FOREIGN KEY (inv_dashboard_page_id) REFERENCES public.dashboard_pages(id) ON DELETE CASCADE;


--
-- TOC entry 3639 (class 2606 OID 213894)
-- Name: dashboard_pages dashboard_pages_updated_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.dashboard_pages
    ADD CONSTRAINT dashboard_pages_updated_by_id_fk FOREIGN KEY (updated_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3640 (class 2606 OID 213899)
-- Name: download_center_pages download_center_pages_created_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.download_center_pages
    ADD CONSTRAINT download_center_pages_created_by_id_fk FOREIGN KEY (created_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3653 (class 2606 OID 213974)
-- Name: download_center_pages_components download_center_pages_entity_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.download_center_pages_components
    ADD CONSTRAINT download_center_pages_entity_fk FOREIGN KEY (entity_id) REFERENCES public.download_center_pages(id) ON DELETE CASCADE;


--
-- TOC entry 3654 (class 2606 OID 213979)
-- Name: download_center_pages_localizations_links download_center_pages_localizations_links_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.download_center_pages_localizations_links
    ADD CONSTRAINT download_center_pages_localizations_links_fk FOREIGN KEY (download_center_page_id) REFERENCES public.download_center_pages(id) ON DELETE CASCADE;


--
-- TOC entry 3655 (class 2606 OID 213984)
-- Name: download_center_pages_localizations_links download_center_pages_localizations_links_inv_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.download_center_pages_localizations_links
    ADD CONSTRAINT download_center_pages_localizations_links_inv_fk FOREIGN KEY (inv_download_center_page_id) REFERENCES public.download_center_pages(id) ON DELETE CASCADE;


--
-- TOC entry 3641 (class 2606 OID 213904)
-- Name: download_center_pages download_center_pages_updated_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.download_center_pages
    ADD CONSTRAINT download_center_pages_updated_by_id_fk FOREIGN KEY (updated_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3606 (class 2606 OID 16846)
-- Name: files files_created_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.files
    ADD CONSTRAINT files_created_by_id_fk FOREIGN KEY (created_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3608 (class 2606 OID 16851)
-- Name: files_folder_links files_folder_links_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.files_folder_links
    ADD CONSTRAINT files_folder_links_fk FOREIGN KEY (file_id) REFERENCES public.files(id) ON DELETE CASCADE;


--
-- TOC entry 3609 (class 2606 OID 16856)
-- Name: files_folder_links files_folder_links_inv_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.files_folder_links
    ADD CONSTRAINT files_folder_links_inv_fk FOREIGN KEY (folder_id) REFERENCES public.upload_folders(id) ON DELETE CASCADE;


--
-- TOC entry 3610 (class 2606 OID 16861)
-- Name: files_related_morphs files_related_morphs_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.files_related_morphs
    ADD CONSTRAINT files_related_morphs_fk FOREIGN KEY (file_id) REFERENCES public.files(id) ON DELETE CASCADE;


--
-- TOC entry 3607 (class 2606 OID 16866)
-- Name: files files_updated_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.files
    ADD CONSTRAINT files_updated_by_id_fk FOREIGN KEY (updated_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3611 (class 2606 OID 16871)
-- Name: home_pages home_pages_created_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.home_pages
    ADD CONSTRAINT home_pages_created_by_id_fk FOREIGN KEY (created_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3613 (class 2606 OID 16876)
-- Name: home_pages_components home_pages_entity_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.home_pages_components
    ADD CONSTRAINT home_pages_entity_fk FOREIGN KEY (entity_id) REFERENCES public.home_pages(id) ON DELETE CASCADE;


--
-- TOC entry 3614 (class 2606 OID 16881)
-- Name: home_pages_localizations_links home_pages_localizations_links_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.home_pages_localizations_links
    ADD CONSTRAINT home_pages_localizations_links_fk FOREIGN KEY (home_page_id) REFERENCES public.home_pages(id) ON DELETE CASCADE;


--
-- TOC entry 3615 (class 2606 OID 16886)
-- Name: home_pages_localizations_links home_pages_localizations_links_inv_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.home_pages_localizations_links
    ADD CONSTRAINT home_pages_localizations_links_inv_fk FOREIGN KEY (inv_home_page_id) REFERENCES public.home_pages(id) ON DELETE CASCADE;


--
-- TOC entry 3612 (class 2606 OID 16891)
-- Name: home_pages home_pages_updated_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.home_pages
    ADD CONSTRAINT home_pages_updated_by_id_fk FOREIGN KEY (updated_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3616 (class 2606 OID 16896)
-- Name: i18n_locale i18n_locale_created_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.i18n_locale
    ADD CONSTRAINT i18n_locale_created_by_id_fk FOREIGN KEY (created_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3617 (class 2606 OID 16901)
-- Name: i18n_locale i18n_locale_updated_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.i18n_locale
    ADD CONSTRAINT i18n_locale_updated_by_id_fk FOREIGN KEY (updated_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3642 (class 2606 OID 213919)
-- Name: news_pages news_pages_created_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.news_pages
    ADD CONSTRAINT news_pages_created_by_id_fk FOREIGN KEY (created_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3656 (class 2606 OID 214004)
-- Name: news_pages_components news_pages_entity_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.news_pages_components
    ADD CONSTRAINT news_pages_entity_fk FOREIGN KEY (entity_id) REFERENCES public.news_pages(id) ON DELETE CASCADE;


--
-- TOC entry 3657 (class 2606 OID 214009)
-- Name: news_pages_localizations_links news_pages_localizations_links_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.news_pages_localizations_links
    ADD CONSTRAINT news_pages_localizations_links_fk FOREIGN KEY (news_page_id) REFERENCES public.news_pages(id) ON DELETE CASCADE;


--
-- TOC entry 3658 (class 2606 OID 214014)
-- Name: news_pages_localizations_links news_pages_localizations_links_inv_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.news_pages_localizations_links
    ADD CONSTRAINT news_pages_localizations_links_inv_fk FOREIGN KEY (inv_news_page_id) REFERENCES public.news_pages(id) ON DELETE CASCADE;


--
-- TOC entry 3643 (class 2606 OID 213924)
-- Name: news_pages news_pages_updated_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.news_pages
    ADD CONSTRAINT news_pages_updated_by_id_fk FOREIGN KEY (updated_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3688 (class 2606 OID 214542)
-- Name: order_item_pages order_item_pages_created_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_item_pages
    ADD CONSTRAINT order_item_pages_created_by_id_fk FOREIGN KEY (created_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3690 (class 2606 OID 214552)
-- Name: order_item_pages_components order_item_pages_entity_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_item_pages_components
    ADD CONSTRAINT order_item_pages_entity_fk FOREIGN KEY (entity_id) REFERENCES public.order_item_pages(id) ON DELETE CASCADE;


--
-- TOC entry 3691 (class 2606 OID 214557)
-- Name: order_item_pages_localizations_links order_item_pages_localizations_links_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_item_pages_localizations_links
    ADD CONSTRAINT order_item_pages_localizations_links_fk FOREIGN KEY (order_item_page_id) REFERENCES public.order_item_pages(id) ON DELETE CASCADE;


--
-- TOC entry 3692 (class 2606 OID 214562)
-- Name: order_item_pages_localizations_links order_item_pages_localizations_links_inv_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_item_pages_localizations_links
    ADD CONSTRAINT order_item_pages_localizations_links_inv_fk FOREIGN KEY (inv_order_item_page_id) REFERENCES public.order_item_pages(id) ON DELETE CASCADE;


--
-- TOC entry 3689 (class 2606 OID 214547)
-- Name: order_item_pages order_item_pages_updated_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_item_pages
    ADD CONSTRAINT order_item_pages_updated_by_id_fk FOREIGN KEY (updated_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3644 (class 2606 OID 213929)
-- Name: order_pages order_pages_created_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_pages
    ADD CONSTRAINT order_pages_created_by_id_fk FOREIGN KEY (created_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3659 (class 2606 OID 214019)
-- Name: order_pages_components order_pages_entity_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_pages_components
    ADD CONSTRAINT order_pages_entity_fk FOREIGN KEY (entity_id) REFERENCES public.order_pages(id) ON DELETE CASCADE;


--
-- TOC entry 3660 (class 2606 OID 214024)
-- Name: order_pages_localizations_links order_pages_localizations_links_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_pages_localizations_links
    ADD CONSTRAINT order_pages_localizations_links_fk FOREIGN KEY (order_page_id) REFERENCES public.order_pages(id) ON DELETE CASCADE;


--
-- TOC entry 3661 (class 2606 OID 214029)
-- Name: order_pages_localizations_links order_pages_localizations_links_inv_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_pages_localizations_links
    ADD CONSTRAINT order_pages_localizations_links_inv_fk FOREIGN KEY (inv_order_page_id) REFERENCES public.order_pages(id) ON DELETE CASCADE;


--
-- TOC entry 3645 (class 2606 OID 213934)
-- Name: order_pages order_pages_updated_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_pages
    ADD CONSTRAINT order_pages_updated_by_id_fk FOREIGN KEY (updated_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3646 (class 2606 OID 213939)
-- Name: orders_pages orders_pages_created_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orders_pages
    ADD CONSTRAINT orders_pages_created_by_id_fk FOREIGN KEY (created_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3662 (class 2606 OID 214034)
-- Name: orders_pages_components orders_pages_entity_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orders_pages_components
    ADD CONSTRAINT orders_pages_entity_fk FOREIGN KEY (entity_id) REFERENCES public.orders_pages(id) ON DELETE CASCADE;


--
-- TOC entry 3663 (class 2606 OID 214039)
-- Name: orders_pages_localizations_links orders_pages_localizations_links_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orders_pages_localizations_links
    ADD CONSTRAINT orders_pages_localizations_links_fk FOREIGN KEY (orders_page_id) REFERENCES public.orders_pages(id) ON DELETE CASCADE;


--
-- TOC entry 3664 (class 2606 OID 214044)
-- Name: orders_pages_localizations_links orders_pages_localizations_links_inv_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orders_pages_localizations_links
    ADD CONSTRAINT orders_pages_localizations_links_inv_fk FOREIGN KEY (inv_orders_page_id) REFERENCES public.orders_pages(id) ON DELETE CASCADE;


--
-- TOC entry 3647 (class 2606 OID 213944)
-- Name: orders_pages orders_pages_updated_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orders_pages
    ADD CONSTRAINT orders_pages_updated_by_id_fk FOREIGN KEY (updated_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3648 (class 2606 OID 213949)
-- Name: outlet_pages outlet_pages_created_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.outlet_pages
    ADD CONSTRAINT outlet_pages_created_by_id_fk FOREIGN KEY (created_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3665 (class 2606 OID 214049)
-- Name: outlet_pages_components outlet_pages_entity_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.outlet_pages_components
    ADD CONSTRAINT outlet_pages_entity_fk FOREIGN KEY (entity_id) REFERENCES public.outlet_pages(id) ON DELETE CASCADE;


--
-- TOC entry 3666 (class 2606 OID 214054)
-- Name: outlet_pages_localizations_links outlet_pages_localizations_links_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.outlet_pages_localizations_links
    ADD CONSTRAINT outlet_pages_localizations_links_fk FOREIGN KEY (outlet_page_id) REFERENCES public.outlet_pages(id) ON DELETE CASCADE;


--
-- TOC entry 3667 (class 2606 OID 214059)
-- Name: outlet_pages_localizations_links outlet_pages_localizations_links_inv_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.outlet_pages_localizations_links
    ADD CONSTRAINT outlet_pages_localizations_links_inv_fk FOREIGN KEY (inv_outlet_page_id) REFERENCES public.outlet_pages(id) ON DELETE CASCADE;


--
-- TOC entry 3649 (class 2606 OID 213954)
-- Name: outlet_pages outlet_pages_updated_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.outlet_pages
    ADD CONSTRAINT outlet_pages_updated_by_id_fk FOREIGN KEY (updated_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3678 (class 2606 OID 214311)
-- Name: privacy_policy_pages privacy_policy_pages_created_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.privacy_policy_pages
    ADD CONSTRAINT privacy_policy_pages_created_by_id_fk FOREIGN KEY (created_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3680 (class 2606 OID 214321)
-- Name: privacy_policy_pages_components privacy_policy_pages_entity_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.privacy_policy_pages_components
    ADD CONSTRAINT privacy_policy_pages_entity_fk FOREIGN KEY (entity_id) REFERENCES public.privacy_policy_pages(id) ON DELETE CASCADE;


--
-- TOC entry 3681 (class 2606 OID 214326)
-- Name: privacy_policy_pages_localizations_links privacy_policy_pages_localizations_links_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.privacy_policy_pages_localizations_links
    ADD CONSTRAINT privacy_policy_pages_localizations_links_fk FOREIGN KEY (privacy_policy_page_id) REFERENCES public.privacy_policy_pages(id) ON DELETE CASCADE;


--
-- TOC entry 3682 (class 2606 OID 214331)
-- Name: privacy_policy_pages_localizations_links privacy_policy_pages_localizations_links_inv_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.privacy_policy_pages_localizations_links
    ADD CONSTRAINT privacy_policy_pages_localizations_links_inv_fk FOREIGN KEY (inv_privacy_policy_page_id) REFERENCES public.privacy_policy_pages(id) ON DELETE CASCADE;


--
-- TOC entry 3679 (class 2606 OID 214316)
-- Name: privacy_policy_pages privacy_policy_pages_updated_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.privacy_policy_pages
    ADD CONSTRAINT privacy_policy_pages_updated_by_id_fk FOREIGN KEY (updated_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3673 (class 2606 OID 214221)
-- Name: regulations_pages regulations_pages_created_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.regulations_pages
    ADD CONSTRAINT regulations_pages_created_by_id_fk FOREIGN KEY (created_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3675 (class 2606 OID 214231)
-- Name: regulations_pages_components regulations_pages_entity_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.regulations_pages_components
    ADD CONSTRAINT regulations_pages_entity_fk FOREIGN KEY (entity_id) REFERENCES public.regulations_pages(id) ON DELETE CASCADE;


--
-- TOC entry 3676 (class 2606 OID 214236)
-- Name: regulations_pages_localizations_links regulations_pages_localizations_links_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.regulations_pages_localizations_links
    ADD CONSTRAINT regulations_pages_localizations_links_fk FOREIGN KEY (regulations_page_id) REFERENCES public.regulations_pages(id) ON DELETE CASCADE;


--
-- TOC entry 3677 (class 2606 OID 214241)
-- Name: regulations_pages_localizations_links regulations_pages_localizations_links_inv_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.regulations_pages_localizations_links
    ADD CONSTRAINT regulations_pages_localizations_links_inv_fk FOREIGN KEY (inv_regulations_page_id) REFERENCES public.regulations_pages(id) ON DELETE CASCADE;


--
-- TOC entry 3674 (class 2606 OID 214226)
-- Name: regulations_pages regulations_pages_updated_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.regulations_pages
    ADD CONSTRAINT regulations_pages_updated_by_id_fk FOREIGN KEY (updated_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3693 (class 2606 OID 214634)
-- Name: search_products_pages search_products_pages_created_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.search_products_pages
    ADD CONSTRAINT search_products_pages_created_by_id_fk FOREIGN KEY (created_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3695 (class 2606 OID 214644)
-- Name: search_products_pages_components search_products_pages_entity_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.search_products_pages_components
    ADD CONSTRAINT search_products_pages_entity_fk FOREIGN KEY (entity_id) REFERENCES public.search_products_pages(id) ON DELETE CASCADE;


--
-- TOC entry 3696 (class 2606 OID 214649)
-- Name: search_products_pages_localizations_links search_products_pages_localizations_links_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.search_products_pages_localizations_links
    ADD CONSTRAINT search_products_pages_localizations_links_fk FOREIGN KEY (search_products_page_id) REFERENCES public.search_products_pages(id) ON DELETE CASCADE;


--
-- TOC entry 3697 (class 2606 OID 214654)
-- Name: search_products_pages_localizations_links search_products_pages_localizations_links_inv_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.search_products_pages_localizations_links
    ADD CONSTRAINT search_products_pages_localizations_links_inv_fk FOREIGN KEY (inv_search_products_page_id) REFERENCES public.search_products_pages(id) ON DELETE CASCADE;


--
-- TOC entry 3694 (class 2606 OID 214639)
-- Name: search_products_pages search_products_pages_updated_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.search_products_pages
    ADD CONSTRAINT search_products_pages_updated_by_id_fk FOREIGN KEY (updated_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3618 (class 2606 OID 16906)
-- Name: strapi_api_token_permissions strapi_api_token_permissions_created_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.strapi_api_token_permissions
    ADD CONSTRAINT strapi_api_token_permissions_created_by_id_fk FOREIGN KEY (created_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3620 (class 2606 OID 16911)
-- Name: strapi_api_token_permissions_token_links strapi_api_token_permissions_token_links_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.strapi_api_token_permissions_token_links
    ADD CONSTRAINT strapi_api_token_permissions_token_links_fk FOREIGN KEY (api_token_permission_id) REFERENCES public.strapi_api_token_permissions(id) ON DELETE CASCADE;


--
-- TOC entry 3621 (class 2606 OID 16916)
-- Name: strapi_api_token_permissions_token_links strapi_api_token_permissions_token_links_inv_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.strapi_api_token_permissions_token_links
    ADD CONSTRAINT strapi_api_token_permissions_token_links_inv_fk FOREIGN KEY (api_token_id) REFERENCES public.strapi_api_tokens(id) ON DELETE CASCADE;


--
-- TOC entry 3619 (class 2606 OID 16921)
-- Name: strapi_api_token_permissions strapi_api_token_permissions_updated_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.strapi_api_token_permissions
    ADD CONSTRAINT strapi_api_token_permissions_updated_by_id_fk FOREIGN KEY (updated_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3622 (class 2606 OID 16926)
-- Name: strapi_api_tokens strapi_api_tokens_created_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.strapi_api_tokens
    ADD CONSTRAINT strapi_api_tokens_created_by_id_fk FOREIGN KEY (created_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3623 (class 2606 OID 16931)
-- Name: strapi_api_tokens strapi_api_tokens_updated_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.strapi_api_tokens
    ADD CONSTRAINT strapi_api_tokens_updated_by_id_fk FOREIGN KEY (updated_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3624 (class 2606 OID 16936)
-- Name: up_permissions up_permissions_created_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.up_permissions
    ADD CONSTRAINT up_permissions_created_by_id_fk FOREIGN KEY (created_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3626 (class 2606 OID 16941)
-- Name: up_permissions_role_links up_permissions_role_links_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.up_permissions_role_links
    ADD CONSTRAINT up_permissions_role_links_fk FOREIGN KEY (permission_id) REFERENCES public.up_permissions(id) ON DELETE CASCADE;


--
-- TOC entry 3627 (class 2606 OID 16946)
-- Name: up_permissions_role_links up_permissions_role_links_inv_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.up_permissions_role_links
    ADD CONSTRAINT up_permissions_role_links_inv_fk FOREIGN KEY (role_id) REFERENCES public.up_roles(id) ON DELETE CASCADE;


--
-- TOC entry 3625 (class 2606 OID 16951)
-- Name: up_permissions up_permissions_updated_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.up_permissions
    ADD CONSTRAINT up_permissions_updated_by_id_fk FOREIGN KEY (updated_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3628 (class 2606 OID 16956)
-- Name: up_roles up_roles_created_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.up_roles
    ADD CONSTRAINT up_roles_created_by_id_fk FOREIGN KEY (created_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3629 (class 2606 OID 16961)
-- Name: up_roles up_roles_updated_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.up_roles
    ADD CONSTRAINT up_roles_updated_by_id_fk FOREIGN KEY (updated_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3630 (class 2606 OID 16966)
-- Name: up_users up_users_created_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.up_users
    ADD CONSTRAINT up_users_created_by_id_fk FOREIGN KEY (created_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3632 (class 2606 OID 16971)
-- Name: up_users_role_links up_users_role_links_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.up_users_role_links
    ADD CONSTRAINT up_users_role_links_fk FOREIGN KEY (user_id) REFERENCES public.up_users(id) ON DELETE CASCADE;


--
-- TOC entry 3633 (class 2606 OID 16976)
-- Name: up_users_role_links up_users_role_links_inv_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.up_users_role_links
    ADD CONSTRAINT up_users_role_links_inv_fk FOREIGN KEY (role_id) REFERENCES public.up_roles(id) ON DELETE CASCADE;


--
-- TOC entry 3631 (class 2606 OID 16981)
-- Name: up_users up_users_updated_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.up_users
    ADD CONSTRAINT up_users_updated_by_id_fk FOREIGN KEY (updated_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3634 (class 2606 OID 16986)
-- Name: upload_folders upload_folders_created_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.upload_folders
    ADD CONSTRAINT upload_folders_created_by_id_fk FOREIGN KEY (created_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


--
-- TOC entry 3636 (class 2606 OID 16991)
-- Name: upload_folders_parent_links upload_folders_parent_links_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.upload_folders_parent_links
    ADD CONSTRAINT upload_folders_parent_links_fk FOREIGN KEY (folder_id) REFERENCES public.upload_folders(id) ON DELETE CASCADE;


--
-- TOC entry 3637 (class 2606 OID 16996)
-- Name: upload_folders_parent_links upload_folders_parent_links_inv_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.upload_folders_parent_links
    ADD CONSTRAINT upload_folders_parent_links_inv_fk FOREIGN KEY (inv_folder_id) REFERENCES public.upload_folders(id) ON DELETE CASCADE;


--
-- TOC entry 3635 (class 2606 OID 17001)
-- Name: upload_folders upload_folders_updated_by_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.upload_folders
    ADD CONSTRAINT upload_folders_updated_by_id_fk FOREIGN KEY (updated_by_id) REFERENCES public.admin_users(id) ON DELETE SET NULL;


-- Completed on 2022-12-14 13:30:09

--
-- PostgreSQL database dump complete
--

